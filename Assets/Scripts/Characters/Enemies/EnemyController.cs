using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyController : MonoBehaviour,ITurnable
{
    public EnemyInfo enemyInfo;

    [SerializeField] public Animator anim;
    
    
    [Title("Stats")]
    [SerializeField]private int health;
    [SerializeField]private int maxHealth;
    [SerializeField]private int damage;
    [SerializeField] private int defense;
    [SerializeField] private int speed;
    public void OnSpawn(EnemyInfo info)
    {
        enemyInfo = info;
        health = info.health;
        maxHealth = info.health;
        damage = info.damage;
        defense = info.defense;
        Instantiate(enemyInfo.prefab, transform.position,transform.rotation,transform);
        
        anim = GetComponentInChildren<Animator>();
    }
    
    public void TakeDamage(int damageDealt)
    {
        health -= damageDealt - defense;
        if (health <= 0)
        {
            OnDeath();
        }
    }

    private void StartTurn()
    {
        Debug.LogError("Not Implemented");
    }
    public void HitAnimation()
    {
        anim.SetTrigger("Hit");
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

    public void SetSpeed(int value)
    { 
        speed = value;
    }
    public int ReturnSpeed()
    {
        return speed;
    }
    

    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public void TakeTurn()
    {
        StartTurn();
    }
    public void EndTurn()
    {
        
    }
}