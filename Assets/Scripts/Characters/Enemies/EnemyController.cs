using System.Collections;
using System.Collections.Generic;
using Characters;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyController : MonoBehaviour, ITurnable
{
    public EnemyInfo enemyInfo;

    [SerializeField] public Animator anim;


    [Title("Stats")] [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int damage;
    [SerializeField] private int defense;
    [SerializeField] private int speed;
    [SerializeField]private Vector3 originalPosition;
    
    
    public void OnSpawn(EnemyInfo info)
    {
        enemyInfo = info;
        health = info.health;
        maxHealth = info.health;
        damage = info.damage;
        defense = info.defense;
        speed = enemyInfo.baseSpeed;
        originalPosition = transform.position;
        
        Instantiate(enemyInfo.prefab, transform.position, transform.rotation, transform);

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
        Debug.Log("Enemies turn");
        
        var characters = CombatManager.instance.GetCharacters();

        var random = Random.Range(0, characters.Length);

        var target = characters[random];

        StartCoroutine(WaitToAttack(target));
    }


    private IEnumerator WaitToAttack(CharacterController target)
    {
        Debug.Log("WaitToAttack");
        
        yield return new WaitForSeconds(1f);
        var position = target.transform.position + (target.transform.forward + new Vector3(1, 0, 0));

        transform.DOMove(position, 1f);
        anim.SetTrigger("run");
        yield return new WaitForSeconds(1f);
        
        AttackAnimation(target);
    }

    private void AttackAnimation(CharacterController character)
    {
        
        Debug.Log("AttackAnimation");
        
        anim.SetTrigger("attack");

        var damage1 = enemyInfo.damage * 1.2f;

        character.GetHit((int)damage1);

        StartCoroutine(WaitToEndTurn());
    }
    
    IEnumerator WaitToEndTurn()
    {
        yield return new WaitForSeconds(1f);
        EndTurn();
    }

    public void EndTurn()
    {
        transform.DOMove(originalPosition, 1f);
        CombatManager.instance.OnNewTurn();
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