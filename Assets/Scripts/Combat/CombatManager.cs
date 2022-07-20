using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    public CombatState currentState;
    [SerializeField] private int actionIndex;
    [SerializeField] private int enemyIndex;
    [SerializeField] private Queue<ITurnable> turns;
    [SerializeField] private ITurnable currentActor;
    private IState[] states;
    private IState currentStateInstance;
    
    private List<ITurnable> baseActors;
    
    [Title("Character Controllers")] [SerializeField]
    private List<CharacterController> characterControllers;


    [Title("Enemies")] [SerializeField] private List<EnemyController> enemyControllers;

    //TODO: Move this to a separate class
    public EnemyInfo[] availableEnemies;

    private void Start()
    {
        instance = this;
        turns = new Queue<ITurnable>();
        baseActors = new List<ITurnable>();
        InitializeStates();
        
        foreach (var character in characterControllers)
        {
            CombatUIManager.instance.CreateCharacterInfo(character);
            character.ReturnInfo().EndTurn();
            baseActors.Add(character);
        }
        foreach(var enemy in enemyControllers)
            baseActors.Add(enemy);
        
        
        Debug.Log("Base Actors: " + baseActors.Count);
        
        //ShuffleNewTurns();
        SpawnEnemies();
        currentActor = turns.Dequeue();
    }

    private void InitializeStates()
    {
        states = new IState[4];
        states[0] = new Attack();
        states[1] = new Defend();
        states[2] = null;
        states[3] = null;


        currentStateInstance = states[0];
    }


    #region Player Turn

    public CharacterController ReturnCurrentCharacter()
    {
        return currentActor as CharacterController;
    }
    
    private void OnNewTurn()
    {
        if (turns.Count > 0)
        {
            currentActor.TakeTurn();
            currentState = CombatState.SelectingAction;
        }
        else
        {
            ShuffleNewTurns();
        }
    }


    /// <summary>
    /// I'm breaking things
    /// </summary>
    private void ShuffleNewTurns()
    {
        var temp= new List<ITurnable>();

        for (var index = 0; index < baseActors.Count; index++)
        {
            var actor = baseActors[index];
            switch (actor)
            {
                case CharacterController character:
                    character.SetSpeed(character.ReturnSpeed() * Random.Range(1, 3));
                    temp.Add(actor);
                    break;
                case EnemyController enemy:
                    enemy.SetSpeed(enemy.ReturnSpeed() * Random.Range(1, 3));
                    temp.Add(actor);
                    break;
            }
            
        }

        //Organize by speed and add to turns queue
        temp.Where(i => i.ReturnSpeed() > 0).OrderByDescending(i => i.ReturnSpeed()).ToList().ForEach(i => turns.Enqueue(i));
        
        OnNewTurn();
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
               
                    ReturnCurrentCharacter().StartAttack(enemyControllers[enemyIndex]);
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
        try
        {
            currentStateInstance.Action();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
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
        turns.Enqueue(currentActor);
        //End the turn
        currentActor.EndTurn();
        //Get the next character
        currentActor = turns.Dequeue();
        currentActor.TakeTurn();
        currentState = CombatState.SelectingAction;
    }

    #endregion


    public void CanAttack()
    {
        ReturnCurrentCharacter().CanTriggerAttack();
    }

    public void CantAttack()
    {
       ReturnCurrentCharacter().CantTriggerAttack();
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
}