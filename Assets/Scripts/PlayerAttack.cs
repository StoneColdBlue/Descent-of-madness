using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [Header("Attack Settings")]
    public KeyCode attackKey = KeyCode.E;
    public float attackRange = 2f;
    public int attackDamage = 10;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;

    [Header("Visuals")]
    public GameObject attackEffect;
    public Transform attackOrigin;

    [Header("Attack parameters")]
    private float lastAttackTime;
    private bool canAttack = true;
    public EnemyAI EnemyAI;

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && canAttack)
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;

        EnemyAI.TakeDamage(10);

        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        if (attackOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackOrigin.position, attackRange);
        }
    }
}
