using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float movementSpeed = 20f;
    [SerializeField] private float rotationalDamp = 2f;
    [SerializeField] private float stopDistance = 2f;
    [SerializeField] private float sharpTurnDistance = 3f;
    [SerializeField] private float avoidanceRadius = 2f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float collisionBackoffDistance = 2f; 
    [SerializeField] private float backoffTime = 0.5f; 

    private bool isBackingOff = false;

    private void Start()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerObject in playerObjects)
        {
            if (playerObject.name == "Drone Body")
            {
                target = playerObject.transform;
                return;
            }
        }
    }

    private void Update()
    {
        if (target == null || isBackingOff) return;

        AvoidOverlapping();
        Turn();
        Move();
    }

    private void Turn()
    {
        Vector3 directionToTarget = target.position - transform.position;

        float damp = rotationalDamp;
        if (directionToTarget.magnitude < sharpTurnDistance)
        {
            damp = rotationalDamp * 2f;
        }

        Quaternion rotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, damp * Time.deltaTime);
    }

    private void Move()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget > stopDistance)
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }
    }

    private void AvoidOverlapping()
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, avoidanceRadius, enemyLayer);

        foreach (Collider enemy in nearbyEnemies)
        {
            if (enemy.gameObject == gameObject) continue;

            Vector3 directionAway = transform.position - enemy.transform.position;

            transform.position += directionAway.normalized * movementSpeed * Time.deltaTime * 0.5f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(HandleCollisionWithPlayer());
        }
    }

    private IEnumerator HandleCollisionWithPlayer()
    {
        isBackingOff = true;

        Vector3 backoffDirection = -transform.forward * collisionBackoffDistance;
        float elapsedTime = 0f;

        while (elapsedTime < backoffTime)
        {
            transform.position += backoffDirection * Time.deltaTime / backoffTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isBackingOff = false;
    }
}
