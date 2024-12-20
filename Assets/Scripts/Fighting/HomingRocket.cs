using UnityEngine;

public class HomingRocket : MonoBehaviour
{
    [Header("Rocket Settings")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float maxHealth = 50f; 
    [SerializeField] private LayerMask targetLayer;

    private Transform target;
    private Transform _transform;
    private Sentry sentry;
    private float currentHealth;

    private void Start()
    {
        _transform = transform;
        currentHealth = maxHealth;

        Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 direction = (target.position - _transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        _transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetSentry(Sentry parentSentry)
    {
        sentry = parentSentry;
    }

    public void OnLaserInteraction(float laserDamage)
    {
        currentHealth -= laserDamage;
        Debug.Log($"Rocket hit by laser! Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            DestroyRocket();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            Debug.Log($"Rocket hit: {other.name}");

            DroneHealth playerHealth = other.GetComponent<DroneHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"Player damaged for {damage} points!");
            }
        }

        DestroyRocket();
    }

    private void DestroyRocket()
    {
        if (sentry != null)
        {
            sentry.OnRocketDestroyed();
        }

        Destroy(gameObject);
    }
}
