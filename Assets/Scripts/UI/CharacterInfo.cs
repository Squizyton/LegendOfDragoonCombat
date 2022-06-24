using System;
using System.Collections;
using System.Collections.Generic;
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


  private void Start()
  {
    EndTurn();
  }

  public void InjectCharacterInfo(CharacterController character)
  {
    nameText.text = character.name;
    currentHPText.text = character.ReturnHealth().ToString();
    maxHPText.text = character.ReturnMaxHealth().ToString();
    currentMPText.text = character.ReturnMana().ToString();
    maxMPText.text = character.ReturnMaxMana().ToString();
    characterIcon.sprite = character.character.playerIcon;
  }

  public void UpdateHP(CharacterController character)
  {
    var newHealth = character.ReturnHealth();
    StartCoroutine(RemoveHealthCoroutine(newHealth));
  }

  private IEnumerator RemoveHealthCoroutine(int newHealth)
  {
    while (currentHP != newHealth)
    {
      yield return new WaitForSeconds(.2f);
      currentHP--;
      currentHPText.text = currentHP.ToString();
    }
  }


  public void BeginTurn()
  {
    characterIcon.color = Color.white;
  }
  
  
  public void EndTurn()
  {
    characterIcon.color = nonActive;
  }
}