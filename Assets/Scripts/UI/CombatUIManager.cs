using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{
    public static CombatUIManager instance;

    [Title("GameObjects")] public GameObject additionTimerUI;
    public CanvasGroup actionBar;
    public CanvasGroup characterbox;
    public GameObject enemyBox;
    public GameObject damageNumber;
    public Transform characterDetailsContainer;
    public CharacterInfo infoPrefab;

    [SerializeField] private Transform[] actionPictures;
    [SerializeField] private Transform actionCircle;

    //TODO: This could honestly be an array, but for now seperate variables
    [Title("Health Colors")] [SerializeField]
    private Color moreThanHalfHealthColor;

    [SerializeField] private Color lessThanHalfHealthColor;
    [SerializeField] private Color quarterHealthColor;

    [SerializeField] public Transform healthTriangle;


    [Title("Text Objects")] [SerializeField]
    private TextMeshProUGUI enemyName;

    private void Awake() => instance = this;


    public void CreateCharacterInfo(PlayerController player)
    {
        var info = Instantiate(infoPrefab, characterDetailsContainer);
        info.InjectCharacterInfo(player);
        player.InjectInfo(info);
    }

    #region Active Status's

    public void TurnOnCharacterUI()
    {
        characterbox.alpha = 1;
        actionBar.alpha = 1;
        healthTriangle.transform.position = Vector3.zero;
        enemyBox.gameObject.SetActive(false);
    }

    public void TurnOffCharacterUI()
    {
        actionBar.alpha = 0;
        enemyBox.gameObject.SetActive(false);
    }

    public void TurnOffAttackUI()
    {
        enemyBox.gameObject.SetActive(false);
        healthTriangle.transform.position = Vector3.zero;
    }

    public void TurnOnAttackUI()
    {
        actionBar.alpha = 0;
        //For some odd reason this just..doesn't work if its disabled then re enabled...so we are going to move it
        healthTriangle.transform.position = Vector3.zero;
        enemyBox.gameObject.SetActive(true);
        characterbox.alpha = 0;
    }

    #endregion


    public void UpdateEnemySelection(EnemyController enemy)
    {
        enemyName.SetText(enemy.enemyInfo.enemyName);

        var healthColor = new Color();

        //Get The health percentage of the enemy
        var healthPercentage = (enemy.GetHealth() * 100) / enemy.GetMaxHealth();

        healthColor = healthPercentage switch
        {
            > 50 => moreThanHalfHealthColor,
            <= 50 and > 25 => lessThanHalfHealthColor,
            <= 25 => quarterHealthColor
        };

        healthTriangle.GetComponentInChildren<Image>().color = healthColor;

        healthTriangle.DOMove(enemy.transform.position + new Vector3(0, 3f, 0), 0.15f);
    }

    public void MoveCircle(int index)
    {
        actionCircle.DOMove(actionPictures[index].position, 0.25f);
    }

    public void StartAdditionTimer(float value)
    {
        additionTimerUI.GetComponent<CanvasGroup>().alpha = 1;
        additionTimerUI.GetComponent<Animator>().SetTrigger("Start");
        additionTimerUI.GetComponent<Animator>().speed = value;
    }


    //Change this to Object Pooling later on
    public void SpawnDamageNumber(Transform transform, Color color,int damage)
    {
        var number = Instantiate(damageNumber);
        number.transform.position = transform.position;
        var text = number.GetComponentInChildren<TextMeshProUGUI>();
        number.GetComponent<LookAtTarget>().Intialize(CameraManager.instance.GetCurrentCamera().transform);
        text.color = color;
        text.SetText(damage.ToString());
        number.GetComponent<Rigidbody>().AddForce(number.transform.up * 10, ForceMode.Impulse);
    }
}