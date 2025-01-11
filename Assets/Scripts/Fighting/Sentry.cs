using System.Collections;
using UnityEngine;

public class Sentry : MonoBehaviour
{
    [Header("Rocket Settings")]
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private Transform firePoint1;
    [SerializeField] private Transform firePoint2;
    [SerializeField] private float fireInterval = 2.0f;
    [SerializeField] private int maxFiredRockets = 5;
    private Transform target;

    private int _activeRockets = 0;
    private bool _useFirstFirePoint = true;

    [Header("Turret Settings")]
    [SerializeField] private Transform turretHead;
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private float detectionRange = 100f; 

    private void Start()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerObject in playerObjects)
        {
            if (playerObject.name == "Drone Body")
            {
                target = playerObject.transform;
                break;
            }
        }

        StartCoroutine(FireRocketRoutine());
    }

    private IEnumerator FireRocketRoutine()
    {
        while (true)
        {
            if (IsTargetWithinRange() && _activeRockets < maxFiredRockets)
            {
                FireRocket();
            }
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void Update()
    {
        if (IsTargetWithinRange())
        {
            RotateTurretHead();
        }
    }

    private bool IsTargetWithinRange()
    {
        if (target == null) return false;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        return distanceToTarget <= detectionRange; 
    }

    private void RotateTurretHead()
    {
        if (turretHead == null || target == null) return;

        Vector3 direction = (target.position - turretHead.position).normalized;

        direction.y = 0;

        direction.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        turretHead.rotation = Quaternion.Slerp(turretHead.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void FireRocket()
    {
        if (rocketPrefab != null)
        {
            Transform currentFirePoint = _useFirstFirePoint ? firePoint1 : firePoint2;

            if (currentFirePoint != null)
            {
                GameObject rocket = Instantiate(rocketPrefab, currentFirePoint.position, currentFirePoint.rotation);

                _activeRockets++;

                HomingRocket rocketScript = rocket.GetComponent<HomingRocket>();
                if (rocketScript != null)
                {
                    rocketScript.SetTarget(target);
                    rocketScript.SetSentry(this);
                }
            }

            _useFirstFirePoint = !_useFirstFirePoint;
        }
    }

    public void OnRocketDestroyed()
    {
        _activeRockets = Mathf.Max(0, _activeRockets - 1);
    }
}
