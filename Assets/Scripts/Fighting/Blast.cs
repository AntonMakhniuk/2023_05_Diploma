using UnityEngine;

public class Blast : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private LayerMask targetLayer;

    private Transform _transform; 

    private void Start()
    {
        _transform = transform; 
        Destroy(gameObject, lifetime); 
    }

    private void Update()
    {
        _transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Blast collided with: {other.name}, Layer: {other.gameObject.layer}");

        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            Debug.Log("Target is in targetLayer!");

            DroneHealth playerHealth = other.GetComponent<DroneHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"Player damaged for {damage} points!");
            }

            Destroy(gameObject); 
        }
        else
        {
            Debug.Log($"Ignored collision with: {other.name}");
        }
    }
}
