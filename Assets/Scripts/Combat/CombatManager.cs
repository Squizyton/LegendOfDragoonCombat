using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    
    
    
    [SerializeField]private CombatState currentState;
    
    public enum CombatState
    {
        SelectingAction,
        SelectingTarget,
    }
}
