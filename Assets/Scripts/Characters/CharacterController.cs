using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public BaseCharacter character;

    public int level;
    public int health;
    public int maxHealth;
    public int mana;
    public float staminaAmount;
    public bool readyToAttack;
    public void Start()
    {
        health = character.maxHP;
        maxHealth = character.maxHP;
        mana = character.maxMP;
    }
    
    //Stamina tick
    
    
    
}