using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    private Transform target; 
    [SerializeField] float movementSpeed = 20f; 
    [SerializeField] float rotationalDamp = 2f; 
    [SerializeField] float stopDistance = 2f; 
    [SerializeField] float sharpTurnDistance = 3f;
    [SerializeField] float avoidanceRadius = 2f; 
    [SerializeField] LayerMask enemyLayer; 


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

}
