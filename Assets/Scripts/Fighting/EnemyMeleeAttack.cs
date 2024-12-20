using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] private float damage = 20f; 
    [SerializeField] private LayerMask targetLayer; 

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            DroneHealth playerHealth = other.GetComponent<DroneHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); 
                Debug.Log($"Player damaged for {damage} points!");
            }
        }
    }
}