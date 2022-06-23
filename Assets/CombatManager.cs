using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    public CombatState currentState;

    
    v

    public enum CombatState
    {
        SelectingAction,
        SelectingTarget,
    }
}
