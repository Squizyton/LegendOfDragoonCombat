using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
        speed = enemyInfo.baseSpeed;
        
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
        var characters = CombatManager.instance.GetCharacters();
        
        var random = Random.Range(0, characters.Length);
        
        var target = characters[random];

        StartCoroutine(WaitToAttack(target));
    }
    
    
    IEnumerator WaitToAttack(CharacterController target)
    {
        yield return new WaitForSeconds(1f);
        transform.DOMove(target.transform.position,1f);
        Attack(target);
    }
    
    public void Attack(CharacterController character)
    {

        WaitToEndTurn();
    }

   IEnumerator WaitToEndTurn()
    {
        yield return new WaitForSeconds(1f);
        EndTurn();
    }

    public void EndTurn()
    {
        
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



    #region Getters and Setters
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
    #endregion
} 
   