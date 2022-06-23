using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class BaseCharacter : ScriptableObject
{
   public Sprite playerIcon;
   public string playerName;
   public int maxHP;
   public int maxMP;
   public int baseDamage;
   
}
