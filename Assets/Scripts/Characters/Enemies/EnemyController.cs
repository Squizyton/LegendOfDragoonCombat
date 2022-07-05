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
    
    public void TakeDamage(int damageDealt)
    {
        health -= damageDealt - defense;
        if (health <= 0)
        {
            OnDeath();
        }
    }
    private void OnDeath()
    {
        //For now just destroy the object
        Destroy(gameObject);
        CombatManager.instance.RemoveEnemy(this);
    }
    public int GetHealth()
    {
        return health;
    }
    
    public int GetMaxHealth()
    {
        return maxHealth;
    }
}