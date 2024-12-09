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


    public void TakeDamage(float damage)
    {
        currentHealth -= damage; 
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        Debug.Log(damage);
        if (currentHealth <= 0)
        {

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
