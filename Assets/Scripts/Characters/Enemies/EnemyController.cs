using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyInfo enemyInfo;
    
    
    [Title("Stats")]
    [SerializeField]private int health;
    [SerializeField]private int maxHealth;
    [SerializeField]private int damage;
    [SerializeField] private int defense;
    public void OnSpawn(EnemyInfo info)
    {
        enemyInfo = info;
        health = info.health;
        maxHealth = info.health;
        damage = info.damage;
        defense = info.defense;
        Instantiate(enemyInfo.prefab, transform.position,transform.rotation,transform);
    }
    
    
    void OnTurnStart()
    {
    }
    
    
    
    void OnTurnEnd()
    {
    }
    
}