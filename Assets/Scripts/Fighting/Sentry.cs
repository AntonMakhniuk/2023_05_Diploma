using System.Collections;
using UnityEngine;

public class Sentry : MonoBehaviour
{
    [Header("Rocket Settings")]
    [SerializeField] private GameObject rocketPrefab; 
    [SerializeField] private Transform firePoint1; 
    [SerializeField] private Transform firePoint2; 
    [SerializeField] private float fireInterval = 2.0f; 
    [SerializeField] private Transform target; 
    [SerializeField] private int maxFiredRockets = 5; 

    private int _activeRockets = 0; 
    private bool _isFiring = false; 
    private bool _useFirstFirePoint = true; 

    private void Start()
    {
        StartCoroutine(FireRocketRoutine());
    }

    private IEnumerator FireRocketRoutine()
    {
        while (true)
        {
            if (_activeRockets < maxFiredRockets) 
            {
                FireRocket();
            }
            yield return new WaitForSeconds(fireInterval); 
        }
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
