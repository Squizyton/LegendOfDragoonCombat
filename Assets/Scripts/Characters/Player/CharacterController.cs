using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;
public class CharacterController : MonoBehaviour,ITurnable
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

    [Title("Misc Variables")] [SerializeField]
    private VisualEffect defendParticles;
    [SerializeField] private Addition currentAddition;
    
    private Action onHealthChanged;
    private Vector3 originalPosition;

    #region Start Functions
    public void Start()
    {
        health = character.maxHP;
        maxHealth = character.maxHP;
        mana = character.maxMP;
        originalPosition = transform.position;


        currentAddition = character.additions[0];
        AddAnimations();
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
    
    public void SetSpeed(int speed)
    {
        this.speed = speed;
    }

    #endregion
    
    #region Combo System
    public void StartAttack(EnemyController enemy)
    {
        CombatUIManager.instance.TurnOffAttackUI();
        
        anim.enabled = false;
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
    }

    public void Update()
    {
        if (!canIncrementCombo) return;


        if (!Input.GetKeyDown(KeyCode.Space)) return;

        hitOnTime = true;
        HitCombo();
    }

    /// Developers note: Now, normally you'd want to use unity's animator. HOWEVER, Animator does not support add Animations runtime(that I know of currently)...while Animation does.
    /// Don't do this. It's a bad idea. Just use Unity's Animator.
    /// So AS OF RIGHT NOW, I'm just using Animation.Play() to play the animation. I'll fix this later, if I can come up with a better solution.
    /// 
    private void HitCombo()
    {
        //If the combo is greater than the amount of animations, end the combo
        currentCombo++;

        //Play the current combo animation
        animationPlayer.Play(currentAddition.comboList[currentCombo - 1].animationName);
        //Start the timer for the next combo hit
        if (currentCombo < currentAddition.comboList.Count)
            StartCoroutine(WaitForAnimation(currentAddition.comboList[currentCombo - 1].animationSpeed));

        if (currentCombo <= currentAddition.comboList.Count - 1) return;

        StartCoroutine(EndComboDelay(currentAddition.comboList[^1].animation.length));
    }


    private void EndCombo()
    {
        transform.DOKill();
        CameraManager.instance.CombatEnd();
        //Deal damage
        CombatManager.instance.DealDamage(CalculateDamage());
        //End the combo chain and reset the combo
        currentCombo = 0;
        transform.DOMove(originalPosition, .4f);
        CombatManager.instance.NextTurn();
        anim.enabled = true;
    }
    
    private IEnumerator WaitForAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        CombatManager.instance.HitEnemy();
        CombatUIManager.instance.StartAdditionTimer(currentAddition.comboList[currentCombo].animationSpeed);
    }

    private IEnumerator EndComboDelay(float animationTime)
    {
        yield return new WaitForSeconds(animationTime);
        CombatManager.instance.HitEnemy();
        yield return new WaitForSeconds(2f);
        EndCombo();
    }
    
    #endregion


    public void Defend()
    {
        isDefending = true;
        anim.SetTrigger("defend"); 
        defendParticles.Play();
    }

    //Calculate the damage to be dealt
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
    public void GetHit(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
        }

        onHealthChanged?.Invoke();
    }
    
    public void StartTurn()
    {
        //If any weird Coroutines is running, kill it
        StopAllCoroutines();
        
        if (isDefending)
        {
            isDefending = false;
            anim.SetTrigger("idle");
        }
        info.BeginTurn();
    }

    public void EndTurn()
    {
        info.EndTurn();
    }

    public void TakeTurn()
    {
        StartTurn();
    }
}