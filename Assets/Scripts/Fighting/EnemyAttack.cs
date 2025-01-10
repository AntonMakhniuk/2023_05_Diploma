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

        GameObject targetObject = GameObject.FindGameObjectWithTag("Player");

        if (targetObject != null)
        {
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
