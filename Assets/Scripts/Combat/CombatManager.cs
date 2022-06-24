using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    
    [SerializeField]private CombatState currentState;
    [SerializeField] private CombatAction action;
    [SerializeField] private int actionIndex;
    private Queue<CharacterController> characterTurns;
    
    [Title("Character Controllers")]
    [SerializeField] private List<CharacterController> characterControllers;




    private void Start()
    {
        instance = this;
        
        foreach(var character in characterControllers)
            CombatUIManager.instance.CreateCharacterInfo(character);
    }

    private void Update()
    {
        switch (currentState)
        {
            case CombatState.SelectingAction:
                if(Input.GetKeyDown(KeyCode.LeftArrow))
                    SwitchAction(-1);
                else if(Input.GetKeyDown(KeyCode.RightArrow))
                    SwitchAction(1);
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
                if (actionIndex > 3)
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
                    actionIndex = 3;
                }
                break;
            }
        }

        action = (CombatAction)actionIndex;
        CombatUIManager.instance.MoveCircle(actionIndex);
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
