using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    [SerializeField] private CombatState currentState;
    [SerializeField] private CombatAction action;
    [SerializeField] private int actionIndex;
    [SerializeField] private Queue<CharacterController> characterTurns;
    [SerializeField] private CharacterController currentCharacter;

    [Title("Character Controllers")] [SerializeField]
    private List<CharacterController> characterControllers;


    [Title("Enemies")] [SerializeField] private List<EnemyController> enemyControllers;
    //TODO: Move this to a seperate class
    public EnemyInfo[] availableEnemies;

    private void Start()
    {
        instance = this;
        characterTurns = new Queue<CharacterController>();

        foreach (var character in characterControllers)
        {
            Debug.Log(character.name);
            CombatUIManager.instance.CreateCharacterInfo(character);
            character.ReturnInfo().EndTurn();
            characterTurns.Enqueue(character);
        }


        SpawnEnemies();


        currentCharacter = characterTurns.Dequeue();
        OnNewTurn();
    }


    #region Player Turn

    private void OnNewTurn()
    {
        currentCharacter.StartTurn();
        currentState = CombatState.SelectingAction;
    }

    private void Update()
    {
        switch (currentState)
        {
            case CombatState.SelectingAction:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    SwitchAction(-1);
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    SwitchAction(1);


                if (Input.GetKeyDown(KeyCode.Space))
                    NextTurn();
                break;
            case CombatState.SelectingTarget:
                break;
            default:
                Debug.LogError("How did you get here?");
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SwitchAction(float direction)
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

        action = (CombatAction) actionIndex;
        CombatUIManager.instance.MoveCircle(actionIndex);
        //Move the cursor to the selected action
    }

    void NextTurn()
    {
        actionIndex = 0;
        CombatUIManager.instance.MoveCircle(actionIndex);
        //Requeue the character
        characterTurns.Enqueue(currentCharacter);
        //End the turn
        currentCharacter.EndTurn();
        //Get the next character
        currentCharacter = characterTurns.Dequeue();

        currentCharacter.StartTurn();
    }

    #endregion

    
    #region Enemies

    private void SpawnEnemies()
    {
        foreach (var enemy in enemyControllers)
        {
            enemy.OnSpawn(availableEnemies[0]);
        }
    }

    #endregion

    private enum CombatState
    {
        SelectingAction,
        SelectingTarget,
    }

    private enum CombatAction
    {
        Attack,
        Defend,
        Item,
        Flee,
    }
}