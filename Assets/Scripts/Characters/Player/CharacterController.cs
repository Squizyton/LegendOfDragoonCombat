using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Title("The Character")] public BaseCharacter character;


    [Title("The Character's Stats")] [ReadOnly, SerializeField]
    private int level;

    [ReadOnly, SerializeField] private int health;
    [ReadOnly, SerializeField] private int maxHealth;
    [ReadOnly, SerializeField] private int mana;
    [ReadOnly, SerializeField] private int maxMana;
    [ReadOnly, SerializeField] private float staminaAmount;
    [ReadOnly, SerializeField] private bool readyToAttack;
    [ReadOnly, SerializeField] private CharacterInfo info;


    public int currentCombo;
    public int currentDamage;

    bool canIncrementCombo;
    private bool hitOnTime;

    [Title("The Character's Components")] [SerializeField]
    private Animation animation;

    [SerializeField] private Animator anim;

    [SerializeField] private Addition currentAddition;

    private Action onHealthChanged;
    private Vector3 originalPosition;
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
        foreach (var anima in currentAddition.animationList)
        {
           
            animation.AddClip(anima.animation, anima.animationName);
        }
    }


    /// <summary>
    /// To be honest, I could just set all variables to public
    /// </summary>
    /// <returns></returns>

    #region Getters

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

    public float ReturnStaminaAmount()
    {
        return staminaAmount;
    }

    #endregion

    public void StartAttack(EnemyController enemy)
    {
        anim.enabled = false;             
        currentCombo= 0;
       
        
        CombatUIManager.instance.StartAdditionTimer(1f);

        //Get the position of in front of enemy
        var position = enemy.transform.position + (enemy.transform.forward -new Vector3(1 , 0, 0));
        
        
        Debug.Log(currentAddition.animationList[currentCombo].animationSpeed);
        transform.DOMove(position, currentAddition.animationList[currentCombo].animationSpeed);
    }

    public void CanTriggerAttack()
    {
        hitOnTime = false;
        canIncrementCombo = true;
    }

    public void CantTriggerAttack()
    {
        canIncrementCombo = false;

        if (hitOnTime.Equals(false))
            EndCombo();
    }

    public void Update()
    {
        if (!canIncrementCombo) return;


        if (!Input.GetKeyDown(KeyCode.Space)) return;


        hitOnTime = true;
        HitCombo();
    }
        
    /// Developers note: Now, normally you'd want to use unity's animator. HOWEVER, Animator does not support add Animations runtime...while Animation does.
    /// So AS OF RIGHT NOW, I'm just using Animation.Play() to play the animation. I'll fix this later, if I can come up with a better solution.
    /// 
    private void HitCombo()
    {
        //Play the current combo animation
        animation.Play(currentAddition.animationList[currentCombo].animationName);

        //Increase the combo
        currentCombo++;
        //Start the timer for the next combo hit
        CombatUIManager.instance.StartAdditionTimer(currentAddition.animationList[currentCombo].animationSpeed);
    }


    private void EndCombo()
    {
        
        //End the combo chain and reset the combo
        currentCombo = 0;
        //Deal damage
        
        
        transform.DOMove(originalPosition, .4f);
        CombatManager.instance.NextTurn();
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

    public CharacterInfo ReturnInfo()
    {
        return info;
    }

    public void InjectInfo(CharacterInfo ui)
    {
        info = ui;
        onHealthChanged += () => info.UpdateHP(this);
    }

    public void StartTurn()
    {
        info.BeginTurn();
    }

    public void EndTurn()
    {
        info.EndTurn();
    }
}