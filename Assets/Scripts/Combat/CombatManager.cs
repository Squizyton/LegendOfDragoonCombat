using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    public CombatState currentState;
    [SerializeField] private CombatAction action;
    [SerializeField] private int actionIndex;
    [SerializeField] private int enemyIndex;
    [SerializeField] private Queue<CharacterController> characterTurns;
    [SerializeField] private CharacterController currentCharacter;
    private IState[] states;
    private IState currentStateInstance;
    
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
        //TODO: This could be implemented better
        switch (currentState)
        {
            case CombatState.SelectingAction:

                if (Input.GetKeyDown(KeyCode.F))
                    CameraManager.instance.ChangeEnvironmentCamera();

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    SwitchAction(-1);
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    SwitchAction(1);

                if (Input.GetKeyDown(KeyCode.Space))
                    DoAction();
                break;

            case CombatState.SelectingTarget:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    SwitchEnemy(-1);
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    SwitchEnemy(1);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    currentCharacter.StartAttack(enemyControllers[enemyIndex]);
                    currentState = CombatState.Nothing;
                }

                break;
            case CombatState.Nothing:
            default:
                break;
        }
    }

    private void SwitchAction(float direction)
    {
        switch (direction)
        {
            //TODO: Move to a modulo operator
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

        currentStateInstance = states[actionIndex];
      
        //Move the cursor to the selected action
        CombatUIManager.instance.MoveCircle(actionIndex);
      
    }


    private void DoAction()
    {
        currentStateInstance.Action();
    }


    private void SwitchEnemy(float direction)
    {
        switch (direction)
        {
            case >= 1:
                enemyIndex++;
                break;
            case < 1:
                enemyIndex--;
                break;
        }

        enemyIndex = enemyIndex switch
        {
            > 2 => 0,
            < 0 => enemyControllers.Count -1,
            _ => enemyIndex
        };

        CombatUIManager.instance.UpdateEnemySelection(enemyControllers[enemyIndex]);
    }

    
    public void RemoveEnemy(EnemyController enemy)
    {
        enemyControllers.Remove(enemy);
    }
    
    public void NextTurn()
    {
        actionIndex = 0;
        enemyIndex = 0;
        CombatUIManager.instance.TurnOnCharacterUI();
        CombatUIManager.instance.MoveCircle(actionIndex);
        //Requeue the character
        characterTurns.Enqueue(currentCharacter);
        //End the turn
        currentCharacter.EndTurn();
        //Get the next character
        currentCharacter = characterTurns.Dequeue();

        currentCharacter.StartTurn();

        currentState = CombatState.SelectingAction;
    }

    #endregion


    public void CanAttack()
    {
        currentCharacter.CanTriggerAttack();
    }

    public void CantAttack()
    {
        currentCharacter.CantTriggerAttack();
    }

    #region Enemies

    
    public EnemyController[] GetEnemies()
    {
        return enemyControllers.ToArray();
    }
    
    public void DealDamage(int damage)
    {
        enemyControllers[enemyIndex].TakeDamage(damage);
    }

    public void HitEnemy()
    {
        enemyControllers[enemyIndex].HitAnimation();
    }
    
    private void SpawnEnemies()
    {
        foreach (var enemy in enemyControllers)
        {
            enemy.OnSpawn(availableEnemies[0]);
        }
    }

    #endregion

    public enum CombatState
    {
        SelectingAction,
        SelectingTarget,
        Nothing
    }

    private enum CombatAction
    {
        Attack,
        Defend,
        Item,
        Flee,
    }
}