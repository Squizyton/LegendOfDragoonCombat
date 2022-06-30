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


    [SerializeField] private Addition currentAddition;

    private Action onHealthChanged;

    public void Start()
    {
        health = character.maxHP;
        maxHealth = character.maxHP;
        mana = character.maxMP;
        
        
        currentAddition = character.additions[0];


        AddAnimations();
    }

    //Stamina tick
    private void AddAnimations()
    {
        foreach(var anima in currentAddition.animationList)
        {
            Debug.Log("Adding this shit: " + anima.animation);
            animation.AddClip(anima.animation,anima.animationName);
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
        CombatUIManager.instance.StartAdditionTimer(1f);
        transform.DOMove(enemy.transform.position,currentAddition.animationList[currentCombo].animationSpeed);
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

    private void HitCombo()
    {
        //Play the current combo animation
        animation.Play(currentAddition.animationList[currentCombo].animationName);

        //Increase the combo
        currentCombo++;
        //Start the timer for the next combo hit
        CombatUIManager.instance.StartAdditionTimer(currentAddition.animationList[currentCombo].animationSpeed);
    }



    public void EndCombo()
    {
        
        
        //End the combo chain and reset the combo
        
        //Deal damage
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