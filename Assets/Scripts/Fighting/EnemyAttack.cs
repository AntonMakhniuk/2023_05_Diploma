using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Transform target; 
    private Transform _transform;

    private void Start()
    {
        _transform = transform;

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
        InFront();
    }

    private bool InFront()
    {
        Vector3 directionToTarget = _transform.position - target.position;
        float angle = Vector3.Angle(_transform.forward, directionToTarget);

        if (Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270)
        {
            Debug.DrawLine(_transform.position, target.position, Color.green);
            return true;
        }

        Debug.DrawLine(_transform.position, target.position, Color.yellow);
        return false;
    }
}
