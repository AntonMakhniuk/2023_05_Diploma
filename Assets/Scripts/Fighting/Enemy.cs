using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Slider healthBar; // Reference to the health bar UI
    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        UpdateHealthBarVisibility();
        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBarVisibility()
    {
        healthBar.gameObject.SetActive(currentHealth < maxHealth);
    }

    private void Die()
    {
        // Optional: Add visual effects, sounds, or other logic here.
        Destroy(gameObject); // Destroy the enemy
    }
}
