using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyInfo : ScriptableObject
{
    public string enemyName;
    public GameObject prefab;
    public int health;
    public int damage;
    public int defense;
    public int baseSpeed;
    
    [Title("Items"),InfoBox("CURRENTLY NOT BEING USED. DO NOT USE YET")]
    public List<AvailableItems> items;
}

[System.Serializable]
public abstract class AvailableItems
{
    public string itemName;
    public int amount;
}