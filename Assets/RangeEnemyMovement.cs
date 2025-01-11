using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyMovement : MonoBehaviour
{
    private Transform target; 
    [SerializeField] float movementSpeed = 20f; 
    [SerializeField] float rotationalDamp = 2f; 
    [SerializeField] float stopMinDistance = 10f; 
    [SerializeField] float stopMaxDistance = 20f; 
    [SerializeField] float avoidanceRadius = 2f; 
    [SerializeField] LayerMask enemyLayer; 

    float dodgeDistance = 3f; 
    float dodgeSpeed = 4f; 
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

        
        float damp = rotationalDamp;

        Quaternion rotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, damp * Time.deltaTime);
    }

    private void Move()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget > stopMaxDistance || distanceToTarget < stopMinDistance)
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

        float movement = dodgeSpeed * Time.deltaTime;
        transform.position += dodgeDirection * movement;
        currentDodgeDistance += movement;

        if (currentDodgeDistance >= dodgeDistance)
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
