using UnityEngine;

public class ShipFollowCursor : MonoBehaviour
{
    [SerializeField] private Transform lookTarget;  
    [SerializeField] private float rotationSpeed = 2f;  
    [SerializeField] private float smoothingFactor = 0.1f; 

    void Update()
    {
     
        Vector3 directionToTarget = (lookTarget.position - transform.position).normalized;

    
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        
        float smoothedSpeed = Mathf.Lerp(0f, rotationSpeed, Time.deltaTime / smoothingFactor);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothedSpeed);

    }
}
