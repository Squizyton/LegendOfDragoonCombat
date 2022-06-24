using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUIManager : MonoBehaviour
{
    public static CombatUIManager instance;
    
    
    public Transform characterDetailsContainer;


    private void Awake() => instance = this;
}
