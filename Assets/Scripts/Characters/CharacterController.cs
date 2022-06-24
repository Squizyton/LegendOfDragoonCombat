using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public BaseCharacter character;

    [SerializeField] private int level;
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int mana;
    [SerializeField] private int maxMana;
    [SerializeField] private float staminaAmount;
    [SerializeField] private bool readyToAttack;
    [SerializeField] private CharacterInfo info;

    [SerializeField] private Addition currentAddition;

    private Action onHealthChanged;

    public void Start()
    {
        health = character.maxHP;
        maxHealth = character.maxHP;
        mana = character.maxMP;

       
    }

    //Stamina tick


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

    public void GetHit(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
        }

        onHealthChanged?.Invoke();
    }


    public void InjectInfo(CharacterInfo ui)
    {
        info = ui;
        onHealthChanged += () => info.UpdateHP(this);
    }

}