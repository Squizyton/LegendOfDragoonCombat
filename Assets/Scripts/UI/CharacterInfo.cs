using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour 
{
  [Title("Text Objects")] [SerializeField]
  private TextMeshProUGUI nameText;

  [SerializeField] private TextMeshProUGUI currentHPText;
  [SerializeField] private TextMeshProUGUI maxHPText;
  [SerializeField] private TextMeshProUGUI currentMPText;
  [SerializeField] private TextMeshProUGUI maxMPText;

  [Title("Non text objects")] [SerializeField]
  private Image characterIcon;

  [SerializeField] private Color active;
  [SerializeField] private Color nonActive;
  [SerializeField] private Slider spBar;
  
  private int currentHP;

  
  public void InjectCharacterInfo(PlayerController player)
  {
    nameText.text = player.name;
    currentHP = player.ReturnHealth();
    currentHPText.text = player.ReturnHealth().ToString();
    maxHPText.text = player.ReturnMaxHealth().ToString();
    currentMPText.text = player.ReturnMana().ToString();
    maxMPText.text = player.ReturnMaxMana().ToString();
    characterIcon.sprite = player.character.playerIcon;
  }

  public void UpdateHP(PlayerController player)
  {
    var newHealth = player.ReturnHealth();
    StartCoroutine(RemoveHealthCoroutine(newHealth));
  }

  private IEnumerator RemoveHealthCoroutine(int newHealth)
  {
    while (currentHP != newHealth)
    {
      yield return new WaitForSeconds(.12f);
      currentHP--;
      currentHPText.text = currentHP.ToString();
    }
  }


  public void BeginTurn()
  {
    characterIcon.DOColor(active, .2f);
  }
  
  
  public void EndTurn()
  {
    characterIcon.DOColor(nonActive, .2f);
  }
}