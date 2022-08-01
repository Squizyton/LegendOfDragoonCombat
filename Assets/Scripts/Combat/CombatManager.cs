using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
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


    [Title("UI")] [SerializeField] private CombatUIManager combatUi;

    #region Start Functions

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

        foreach (var enemy in enemyControllers)
            baseActors.Add(enemy);

        SpawnEnemies();
        ShuffleNewTurns();
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

    #endregion


    #region Player Turn

    public CharacterController ReturnCurrentCharacter()
    {
        return currentActor as CharacterController;
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
            < 0 => enemyControllers.Count - 1,
            _ => enemyIndex
        };

        CombatUIManager.instance.UpdateEnemySelection(enemyControllers[enemyIndex]);
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

    #endregion
    
    #region Turn Handlers

    /// <summary>
    /// I'm breaking things
    /// </summary>
    private void ShuffleNewTurns()
    {
        var temp = new List<ITurnable>();

        foreach (var actor in baseActors)
        {
            switch (actor)
            {
                case CharacterController character:
                    character.SetSpeed(character.ReturnSpeed() * Random.Range(1, 3));
                    break;
                case EnemyController enemy:
                    enemy.SetSpeed(enemy.ReturnSpeed() * Random.Range(1, 3));
                    break;
            }


            temp.Add(actor);
        }


        //Order by highest speed first
        temp.Sort((x, y) => y.ReturnSpeed().CompareTo(x.ReturnSpeed()));

        foreach (var actor in temp)
        {
            turns.Enqueue(actor);
        }

        OnNewTurn();
    }

    /// <summary>
    /// To Be called whenever an actor's turn has ended
    /// </summary>
    public void OnNewTurn()
    {
        if (turns.Count > 0)
        {
            NextTurn();
        }
        else
        {
            ShuffleNewTurns();
        }
    }
    
    private void NextTurn()
    {
        //Get the next character
        currentActor = turns.Dequeue();


        switch (currentActor)
        {
            case CharacterController:
                enemyIndex = 0;
                SwitchAction(actionIndex = 0);
                CombatUIManager.instance.TurnOnCharacterUI();
                CombatUIManager.instance.MoveCircle(actionIndex);
                currentState = CombatState.SelectingAction;
                break;
            case EnemyController:
                Debug.Log("Blop");
                currentState = CombatState.Nothing;
                CombatUIManager.instance.TurnOffCharacterUI();
                break;
        }


        currentActor.TakeTurn();
    }


    public void CanAttack()
    {
        ReturnCurrentCharacter().CanTriggerAttack();
    }

    public void CantAttack()
    {
        ReturnCurrentCharacter().CantTriggerAttack();
    }

    #endregion

    #region Enemies

    public EnemyController[] GetEnemies()
    {
        return enemyControllers.ToArray();
    }

    public CharacterController[] GetCharacters()
    {
        return characterControllers.ToArray();
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

    public void RemoveEnemy(EnemyController enemy)
    {
        enemyControllers.Remove(enemy);
    }

    #endregion

    public enum CombatState
    {
        SelectingAction,
        SelectingTarget,
        Nothing
    }
}