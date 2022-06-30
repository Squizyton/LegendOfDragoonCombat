using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{
    public static CombatUIManager instance;

    [Title("GameObjects")] public GameObject additionTimerUI;
    public GameObject actionBar;
    public GameObject characterbox;
    public GameObject enemyBox;
    public Transform characterDetailsContainer;
    public CharacterInfo infoPrefab;

    [SerializeField] private Transform[] actionPictures;
    [SerializeField] private Transform actionCircle;
    
    //TODO: This could honestly be an array, but for now seperate variables
    [Title("Health Colors")]
    [SerializeField]private Color moreThanHalfHealthColor;
    [SerializeField]private Color lessThanHalfHealthColor;
    [SerializeField]private Color quarterHealthColor;

    [SerializeField] public Transform healthTriangle;


    [Title("Text Objects")] [SerializeField]
    private TextMeshProUGUI enemyName;
    
    private void Awake() => instance = this;


    public void CreateCharacterInfo(CharacterController character)
    {
        var info = Instantiate(infoPrefab, characterDetailsContainer);
        info.InjectCharacterInfo(character);
        character.InjectInfo(info);
    }


    public void TurnOnAttackUI()
    {
        actionBar.SetActive(false);
        //For some odd reason this just..doesn't work if its disabled then re enabled...so we are going to move it
        healthTriangle.transform.position = Vector3.zero;
        enemyBox.gameObject.SetActive(true);
        characterbox.SetActive(false);
    }

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
        additionTimerUI.SetActive(true);
        additionTimerUI.GetComponent<Animator>().StartPlayback();
        additionTimerUI.GetComponent<Animator>().speed = value;
    }
}
