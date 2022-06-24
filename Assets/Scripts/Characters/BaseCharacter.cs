using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;



[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class BaseCharacter : ScriptableObject
{
   [Title("Icon")]
   public Sprite playerIcon;
   [Title("Name")]
   public string playerName;
   [Title("Base Stats")]
   public int maxHP;
   public int maxMP;
   public int baseDamage;
   
   [Space]
   [Title("Additions"),ListDrawerSettings(NumberOfItemsPerPage = 1)]
   public List<Addition>additions;
}
