using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float rotationSpeed = 100f; // Speed of rotation around the X-axis

    private void Update()
    {
        // Rotate the blade around the X-axis
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is in the target layer
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            DroneHealth playerHealth = collision.gameObject.GetComponent<DroneHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"Player damaged for {damage} points!");
            }
        }
    }
}
