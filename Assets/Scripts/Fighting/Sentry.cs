using System.Collections;
using UnityEngine;

public class Sentry : MonoBehaviour
{
    [Header("Rocket Settings")]
    [SerializeField] private GameObject rocketPrefab; // Prefab for the rocket
    [SerializeField] private Transform firePoint1; // First fire point
    [SerializeField] private Transform firePoint2; // Second fire point
    [SerializeField] private float fireInterval = 2.0f; // Time interval between firing rockets
    [SerializeField] private Transform target; // Player target
    [SerializeField] private int maxFiredRockets = 5; // Maximum number of active rockets

    private int _activeRockets = 0; // Counter for active rockets
    private bool _isFiring = false; // Ensure only one coroutine runs at a time
    private bool _useFirstFirePoint = true; // Toggle for alternating fire points

    private void Start()
    {
        StartCoroutine(FireRocketRoutine());
    }

    private IEnumerator FireRocketRoutine()
    {
        while (true)
        {
            if (_activeRockets < maxFiredRockets) // Only fire if under max rocket limit
            {
                FireRocket();
            }
            yield return new WaitForSeconds(fireInterval); // Wait for the specified interval
        }
    }

    private void FireRocket()
    {
        if (rocketPrefab != null)
        {
            // Determine the current fire point to use
            Transform currentFirePoint = _useFirstFirePoint ? firePoint1 : firePoint2;

            if (currentFirePoint != null)
            {
                // Instantiate the rocket at the selected fire point
                GameObject rocket = Instantiate(rocketPrefab, currentFirePoint.position, currentFirePoint.rotation);

                // Increment the active rocket count
                _activeRockets++;

                // Assign the target to the rocket
                HomingRocket rocketScript = rocket.GetComponent<HomingRocket>();
                if (rocketScript != null)
                {
                    rocketScript.SetTarget(target);
                    rocketScript.SetSentry(this); // Pass reference to this sentry
                }

                Debug.Log($"Rocket fired from {currentFirePoint.name}!");
            }

            // Toggle fire point for next rocket
            _useFirstFirePoint = !_useFirstFirePoint;
        }
    }

    // Called by the rocket when destroyed
    public void OnRocketDestroyed()
    {
        _activeRockets = Mathf.Max(0, _activeRockets - 1); // Decrement active rocket count safely
    }
}
