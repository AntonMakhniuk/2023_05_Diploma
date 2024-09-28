using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{
    public class ShipFollowCursor : MonoBehaviour
    {
        [SerializeField] private Transform lookTarget;
        [SerializeField] private float rotationSpeed = 2f;

        void Update()
        {
            Vector3 directionToTarget = (lookTarget.position - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
