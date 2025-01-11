using UnityEngine;

public class HomingRocket : MonoBehaviour
{
    [Header("Rocket Settings")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float avoidanceRadius = 2f; 
    [SerializeField] private LayerMask enemyLayer; 
    [SerializeField] private float avoidanceSpeedFactor = 0.5f; 
    private float lifetime = 10f;


    private Transform target;
    private Transform _transform;
    private Sentry sentry;
    private float currentHealth;

    private void Start()
    {
        _transform = transform;
        currentHealth = maxHealth;

        Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
        Invoke(nameof(DestroyRocket), lifetime);
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        AvoidOverlapping();

        Vector3 direction = (target.position - _transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        _transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void AvoidOverlapping()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, avoidanceRadius, enemyLayer);

        foreach (Collider obj in nearbyObjects)
        {
            if (obj.gameObject == gameObject) continue;

            Vector3 directionAway = transform.position - obj.transform.position;

            _transform.position += directionAway.normalized * speed * Time.deltaTime * avoidanceSpeedFactor;

            Debug.Log($"Avoiding object: {obj.name}, Direction Away: {directionAway}");
        }
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
