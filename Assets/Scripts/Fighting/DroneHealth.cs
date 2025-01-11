using UnityEngine;
using UnityEngine.UI;

public class DroneHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject deathScreenPanel; 

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();

        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(false); 
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            TriggerDeath();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    private void TriggerDeath()
    {
        Debug.Log("Drone Destroyed!");

        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(true);
        }

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
