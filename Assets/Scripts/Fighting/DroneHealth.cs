using UnityEngine;
using UnityEngine.UI;

public class DroneHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    [SerializeField] private Slider healthBar; 

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(10f);
        }
    }


    private void TakeDamage(float damagePercentage)
    {
        float damageAmount = maxHealth * (damagePercentage / 100);
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            // Handle the drone being destroyed or game over state
            Debug.Log("Drone Destroyed!");
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }
}
