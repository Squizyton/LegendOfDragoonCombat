using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

namespace Characters.Player
{
    public class PlayerController : MonoBehaviour,ITurnable
    {
        [Title("The Character")] public BaseCharacter character;


        [Title("The Character's Stats")] [ReadOnly, SerializeField]
        private int level;

        [ReadOnly, SerializeField] private int health;
        [ReadOnly, SerializeField] private int maxHealth;
        [ReadOnly, SerializeField] private int mana;
        [ReadOnly, SerializeField] private int maxMana;
        [ReadOnly, SerializeField] private bool readyToAttack;
        [ReadOnly, SerializeField] private int speed;
        [ReadOnly, SerializeField] private CharacterInfo info;
        private bool isDefending;

        public int currentCombo;
        public int currentDamage;

        bool canIncrementCombo;
        private bool hitOnTime;

        [Title("The Character's Components")] 
    
    
    
        [Header("Animation Variables")]
        [SerializeField] private Animator anim;
        [SerializeField]private Animation animationPlayer;
        [SerializeField]private AnimatorOverrideController overrideController;
        
        [Title("Misc Variables")] [SerializeField]
        private VisualEffect defendParticles;
        [SerializeField] private Addition currentAddition;
    
        private Action onHealthChanged;
        private Vector3 originalPosition;

        #region Start Functions
        public void Awake()
        {
            health = character.maxHP;
            maxHealth = character.maxHP;
            mana = character.maxMP;
            originalPosition = transform.position;
            speed = character.baseSpeed;

            currentAddition = character.additions[0];
            //AddAnimations();
            TestNewAnimationAdding();

        }

        //Stamina tick
        private void AddAnimations()
        {
            foreach (var anima in currentAddition.comboList)
            {
                animationPlayer.AddClip(anima.animation, anima.animationName);
            }
        }

        public void InjectInfo(CharacterInfo ui)
        {
            info = ui;
            onHealthChanged += () => info.UpdateHP(this);
        }
        #endregion
    

        /// <summary>
        /// To be honest, I could just set all variables to public
        /// </summary>
        /// <returns></returns>

        #region Getters

    
        public CharacterInfo ReturnInfo()
        {
            return info;
        }
    
        public int ReturnHealth()
        {
            return health;
        }

        public int ReturnMaxHealth()
        {
            return maxHealth;
        }

        public int ReturnMana()
        {
            return mana;
        }

        public float ReturnMaxMana()
        {
            return maxMana;
        }

        public int ReturnSpeed()
        {
            return speed;
        }
    
        public void SetSpeed(int value)
        {
            this.speed = value;
        }

        #endregion
    
        //Start of Character's Turn
        private void StartTurn()
        {
            //If any weird Coroutines is running, kill it
            StopAllCoroutines();
        
            if (isDefending)
            {
                isDefending = false;
                anim.SetBool("defend", isDefending);
            }
            info.BeginTurn();
        }
    
    
        #region Combo System
        public void StartAttack(EnemyController enemy)
        {
            CombatUIManager.instance.TurnOffAttackUI();
        
         
            currentCombo = 0;
            CameraManager.instance.ZoomInOnCharacter(this);

            CombatUIManager.instance.StartAdditionTimer(currentAddition.comboList[currentCombo].animationSpeed);

            //Get the position of in front of enemy
            var position = enemy.transform.position + (enemy.transform.forward - new Vector3(1, 0, 0));
        
            transform.DOMove(position, currentAddition.comboList[currentCombo].animationSpeed / 2);
        }

        public void CanTriggerAttack()
        {
            hitOnTime = false;
            canIncrementCombo = true;
        }

        public void CantTriggerAttack()
        {
            canIncrementCombo = false;

            if (hitOnTime) return;
        
            Debug.Log("Not hit on time");
            EndCombo();
            CombatManager.instance.OnNewTurn();
        }

        public void Update()
        {
            if (!canIncrementCombo) return;


            if (!Input.GetKeyDown(KeyCode.Space)) return;

            hitOnTime = true;
            HitCombo();
        }
        
        private void HitCombo()
        {
            //If the combo is greater than the amount of animations, end the combo
            currentCombo++;
            //Play the current combo animation
            anim.SetTrigger("Attack" + currentCombo);
            //Start the timer for the next combo hit
            if (currentCombo < currentAddition.comboList.Count)
                StartCoroutine(WaitForAnimation(currentAddition.comboList[currentCombo - 1].animationSpeed));

            if (currentCombo <= currentAddition.comboList.Count - 1) return;

            StartCoroutine(EndComboDelay(currentAddition.comboList[^1].animation.length));
        }


        private void EndCombo()
        {
            transform.DOKill();
            //Deal damage
            CombatManager.instance.DealDamage(CalculateDamage());
       
            //End the combo chain and reset the combo
            currentCombo = 0;
        }
    
        private int CalculateDamage()
        {
            var damage = (float) character.baseDamage;

            //If the current combo is 0, deal the base damage
            if (currentCombo <= 0) return (int) damage;
        
        
            //If the current combo is greater than 0, deal the damage of the current combo by multiplying the base damage by the current combo multiplier
            for (var x = 0; x < currentCombo; x++)
            {
                damage *= currentAddition.comboList[x].damageMultiplier;
            }
            return (int)damage;
        }
    
        
        //TODO: Cache WaitForSeconds so we aren't allocating memory every time we call it.
    
        private IEnumerator WaitForAnimation(float time)
        {
            yield return new WaitForSeconds(time);
           
            CombatUIManager.instance.StartAdditionTimer(currentAddition.comboList[currentCombo].animationSpeed);
        }

    
        private IEnumerator EndComboDelay(float animationTime)
        {
            yield return new WaitForSeconds(animationTime);
            EndCombo();
            yield return new WaitForSeconds(2f);
            CameraManager.instance.CombatEnd();
            CombatManager.instance.OnNewTurn();
            transform.DOMove(originalPosition, .4f);
        }
    
        #endregion


        public void EndTurn()
        {
            info.EndTurn();
            CombatManager.instance.OnNewTurn();
        }
        public void Defend()
        {
            isDefending = true;
            anim.SetBool("defend",isDefending); 
            defendParticles.Play();
        }
    
        public void GetHit(int damageTaken)
        {
            anim.SetTrigger("hit");
            
            if (isDefending)
            {
                damageTaken /= 2;
            }
            
            CombatUIManager.instance.SpawnDamageNumber(this.transform,Color.red,damageTaken);
            
            health -= damageTaken;

            

            if (health <= 0)
            {
                health = 0;
            }

            onHealthChanged?.Invoke();
        }
    
        public void TakeTurn()
        {
            StartTurn();
        }

        private void TestNewAnimationAdding()
        {
            var clipOverrides = new AnimationClipOverrides(overrideController.overridesCount);
            overrideController.GetOverrides(clipOverrides);

            var indexString = "";
            
            for(var i = 0; i < currentAddition.comboList.Count; i++)
            {
                
                indexString = "Attack " + (i + 1);
                
                clipOverrides[indexString] = currentAddition.comboList[i].animation;
                
                Debug.Log(clipOverrides[indexString]);
            }
            overrideController.ApplyOverrides(clipOverrides);
            
            
            foreach(var clipName in overrideController.animationClips)
                Debug.Log(clipName);
        }

    }
    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
    {
        public AnimationClipOverrides(int capacity) : base(capacity) {}

        public AnimationClip this[string name]
        {
            get { return this.Find(x => x.Key.name.Equals(name)).Value; }
            set
            {
                int index = this.FindIndex(x => x.Key.name.Equals(name));
                if (index != -1)
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }
}