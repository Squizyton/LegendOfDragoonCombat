using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    
    [SerializeField]private CombatState currentState;
    [SerializeField] private CombatAction action;

    [SerializeField]
    private int actionIndex;

    private Queue<CharacterController> characterTurns;
    
    
    
    
    

    private void Update()
    {
        switch (currentState)
        {
            case CombatState.SelectingAction:
                break;
            case CombatState.SelectingTarget:
                break;
            default:
                Debug.LogError("How did you get here?");
                throw new ArgumentOutOfRangeException();
        }
    }


    public void SwitchAction(float direction)
    {
        //TODO: Move to a modulo operator
        switch (direction)
        {
            case > 0:
            {
                actionIndex++;
                if (actionIndex >= 5)
                {
                    actionIndex = 0;
                }

                break;
            }
            case < 0:
            {
                actionIndex--;
                if (actionIndex < 0)
                {
                    actionIndex = 4;
                }

                break;
            }
        }

        action = (CombatAction)actionIndex;
        
        //Move the cursor to the selected action
    }


    private enum CombatState
        {
            SelectingAction,
            SelectingTarget,
        }

    private enum CombatAction
        {
            Attack,
            Defend,
            Special,
            Item,
            Flee,
        }

}
