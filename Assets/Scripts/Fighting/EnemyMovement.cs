using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform target; // The actual target to follow (DroneBody)
    [SerializeField] float movementSpeed = 20f; // Speed of movement
    [SerializeField] float rotationalDamp = 2f; // Damping factor for rotation
    [SerializeField] float stopDistance = 2f; // Minimum distance to stop moving toward the target
    [SerializeField] float sharpTurnDistance = 3f; // Distance within which turns are sharper
    [SerializeField] float avoidanceRadius = 20f; // Radius for detecting nearby enemies
    [SerializeField] LayerMask enemyLayer; // Layer for enemies


    private void Start()
    {
        // Locate the DroneBody specifically by its tag "Player"
        GameObject targetObject = GameObject.FindGameObjectWithTag("Player");

        if (targetObject != null)
        {
            // Ensure it references the actual DroneBody child
            Transform droneBody = targetObject.transform.Find("DroneBody");
            if (droneBody != null)
            {
                target = droneBody;
            }
            else
            {
                Debug.LogError("DroneBody not found as a child of PlayerDroneSetup!");
            }
        }
        else
        {
            Debug.LogError("Player tag not found in the scene!");
        }
    }

    private void Update()
    {
        if (target == null) return;
        AvoidOverlapping(); // Handle overlapping first
                            // Ensure target is set
        Turn();
        Move();
    }

    private void Turn()
    {
        Vector3 directionToTarget = target.position - transform.position;

        // Sharpen the turn when close to the target
        float damp = rotationalDamp;
        if (directionToTarget.magnitude < sharpTurnDistance)
        {
            damp = rotationalDamp * 4f;
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
        // Check for nearby enemies
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, avoidanceRadius, enemyLayer);

        Debug.Log($"Nearby enemies detected: {nearbyEnemies.Length}");

        foreach (Collider enemy in nearbyEnemies)
        {
            if (enemy.gameObject == gameObject) continue; // Skip itself

            // Calculate direction away from the overlapping enemy
            Vector3 directionAway = transform.position - enemy.transform.position;
            Debug.Log($"Avoiding enemy: {enemy.name}, Direction: {directionAway}");

            // Apply avoidance movement
            transform.position += directionAway.normalized * movementSpeed * Time.deltaTime * 0.5f; // Push away
        }
    }

}
