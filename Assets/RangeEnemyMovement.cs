using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyMovement : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float movementSpeed = 20f;
    [SerializeField] private float rotationalDamp = 2f;
    [SerializeField] private float stopMinDistance = 10f; 
    [SerializeField] private float stopMaxDistance = 20f; 
    [SerializeField] private float avoidanceRadius = 2f;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float dodgeDistance = 5f; 
    [SerializeField] private float dodgeSpeed = 4f; 
    private bool isDodgingRight = true;
    private float currentDodgeDistance = 0f;

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
        if (target == null) return;

        AvoidOverlapping();
        Turn();
        Move();
    }

    private void Turn()
    {
        Vector3 directionToTarget = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(directionToTarget);

        float damp = rotationalDamp;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, damp * Time.deltaTime);
    }

    private void Move()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget < stopMinDistance)
        {
            transform.position -= transform.forward * movementSpeed * Time.deltaTime;
        }
        else if (distanceToTarget > stopMaxDistance)
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }
        else
        {
            Dodge();
        }
    }

    private void Dodge()
    {
        Vector3 dodgeDirection = isDodgingRight ? transform.right : -transform.right;

        float randomSpeedFactor = Random.Range(0.8f, 1.2f); 
        float movement = dodgeSpeed * randomSpeedFactor * Time.deltaTime;
        transform.position += dodgeDirection * movement;

        currentDodgeDistance += movement;

        float randomizedDodgeDistance = dodgeDistance * Random.Range(0.8f, 1.2f);

        if (currentDodgeDistance >= randomizedDodgeDistance)
        {
            isDodgingRight = !isDodgingRight; 
            currentDodgeDistance = 0f;       
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
}