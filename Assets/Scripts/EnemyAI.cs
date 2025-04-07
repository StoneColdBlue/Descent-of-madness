using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection")]
    public float sightRange = 10f;
    public float fieldOfView = 90f;
    public LayerMask obstructionLayers;
    private Transform player;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float chargeSpeed = 8f;
    public float attackRange = 2f;
    public float dashDistance = 5f;
    public float cooldownTime = 2f;
    private bool isCharging;
    private bool inCooldown;

    [Header("Combat")]
    public int attackDamage = 15;
    public float attackRadius = 1.5f;
    public float attackDelay = 0.5f;

    [Header("Health")]
    public int maxHealth = 50;
    private int currentHealth;
    public UnityEvent OnEnemyDeath;
    public UnityEvent OnEnemyDamaged;
    public static PlayerHealth playerHealth;
    private Vector3 lastKnownPlayerPos;
    private float cooldownTimer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0) return;

        if (!inCooldown)
        {
            if (PlayerInSight())
            {
                lastKnownPlayerPos = player.position;

                if (!isCharging)
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                    if (distanceToPlayer <= attackRange)
                    {
                        StartCoroutine(DashAttack());
                    }
                    else
                    {
                        ChargeTowardsPlayer();
                    }
                }
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0) inCooldown = false;
        }
    }

    bool PlayerInSight()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < fieldOfView / 2f)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstructionLayers))
            {
                return true;
            }
        }
        return false;
    }

    void ChargeTowardsPlayer()
    {
        Vector3 direction = (lastKnownPlayerPos - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(new Vector3(lastKnownPlayerPos.x, transform.position.y, lastKnownPlayerPos.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<PlayerHealth>();
        playerHealth = GetComponent<PlayerHealth>();

        if (other.gameObject.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(15);

            }
        }
        
       
    }

    IEnumerator DashAttack()
    {
        isCharging = true;

        // Dash
        Vector3 dashDirection = (player.position - transform.position).normalized;
        float dashStartTime = Time.time;

        while (Time.time < dashStartTime + 0.5f) // Dash duration
        {
            transform.position += dashDirection * chargeSpeed * Time.deltaTime;
            yield return null;
        }

        // Attack
        yield return new WaitForSeconds(attackDelay);
        if (Vector3.Distance(transform.position, player.position) <= attackRadius)
        {
            playerHealth.TakeDamage(15);
            Debug.Log("HP:" + playerHealth);
        }

        // Cooldown
        isCharging = false;
        inCooldown = true;
        cooldownTimer = cooldownTime;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnEnemyDamaged.Invoke();
        Debug.Log("Enemy HP:" + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnEnemyDeath.Invoke();
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Draw sight range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}