using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private float invincibilityTime = 1f;

    [Header("Events")]
    public UnityEvent<int> OnHealthChanged;
    public UnityEvent OnDeath;
    public UnityEvent OnPickup;
    public UnityEvent OnDamageTaken;

    private float lastDamageTime;
    private bool isInvincible;

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            HandlePickup(other.GetComponent<HealthPickup>());
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            
            TakeDamage(15);
            Debug.Log("Player HP:" + currentHealth);

        }
    }

    void HandlePickup(HealthPickup pickup)
    {
        if (pickup != null)
        {
            AddHealth(pickup.healthAmount);
            Destroy(pickup.gameObject);
            OnPickup?.Invoke();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || currentHealth <= 0) return;

        if (Time.time > lastDamageTime + invincibilityTime)
        {
            currentHealth = Mathf.Max(0, currentHealth - damage);
            lastDamageTime = Time.time;
            OnHealthChanged?.Invoke(currentHealth);
            OnDamageTaken?.Invoke();

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(InvincibilityFrame());
            }
        }
    }


    public void AddHealth(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth);
    }

    IEnumerator InvincibilityFrame()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    void Die()
    {
        OnDeath?.Invoke();
        // Add death logic (e.g., respawn, game over)
    }
}

[System.Serializable]
public class HealthPickup : MonoBehaviour
{
    public int healthAmount = 25;
}
