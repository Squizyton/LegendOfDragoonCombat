using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatUIManager : MonoBehaviour
{
    public static CombatUIManager instance;
    
    
    public Transform characterDetailsContainer;
    public CharacterInfo infoPrefab;

    [SerializeField] private Transform[] actionPictures;
    [SerializeField] private Transform actionCircle;
    
    //TODO: This could honestly be an array, but for now seperate variables
    [Title("Health Colors")]
    [SerializeField]private Color moreThanHalfHealthColor;
    [SerializeField]private Color lessThanHalfHealthColor;
    [SerializeField]private Color quarterHealthColor;
    
    
    private void Awake() => instance = this;


    public void CreateCharacterInfo(CharacterController character)
    {
        var info = Instantiate(infoPrefab, characterDetailsContainer);
        info.InjectCharacterInfo(character);
        character.InjectInfo(info);
    }


    public void MoveCircle(int index)
    {
        actionCircle.DOMove(actionPictures[index].position, 0.25f);
    }
}
