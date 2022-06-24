using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUIManager : MonoBehaviour
{
    public static CombatUIManager instance;
    
    
    public Transform characterDetailsContainer;
    public CharacterInfo infoPrefab;

    private void Awake() => instance = this;


    public void CreateCharacterInfo(CharacterController character)
    {
        var info = Instantiate(infoPrefab, characterDetailsContainer);
        info.InjectCharacterInfo(character);
        
        
    }
}
