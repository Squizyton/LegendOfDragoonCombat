using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Title("The Character")]
    public BaseCharacter character;

    [Title("The Character's Stats")]
    [ReadOnly,SerializeField] private int level;
    [ReadOnly,SerializeField] private int health;
    [ReadOnly,SerializeField] private int maxHealth;
    [ReadOnly,SerializeField] private int mana;
    [ReadOnly,SerializeField] private int maxMana;
    [ReadOnly,SerializeField] private float staminaAmount;
    [ReadOnly,SerializeField] private bool readyToAttack;
    [ReadOnly,SerializeField] private CharacterInfo info;

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

    public void StartTurn()
    {
        info.BeginTurn();
    }
}