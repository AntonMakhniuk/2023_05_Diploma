using System.Collections;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [Header("Laser Settings")]
    [SerializeField] private float laserDamagePerSecond = 10f;
    [SerializeField] private LineRenderer beam;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private float maxRange = 50f;
    [SerializeField] private Transform target; 

    [Header("Detection Settings")]
    [SerializeField] private float sightAngle = 90f; 
    [SerializeField] private float detectionRange = 50f;

    private bool _isShooting;
    private IEnumerator _shootCoroutine;

    private void Awake()
    {
        beam.enabled = false;
        beam.startWidth = 0.1f;
        beam.endWidth = 0.1f;
        _shootCoroutine = ShootCoroutine();
    }

    private void Update()
    {
        if (InFront() && HaveLineOfSight())
        {
            if (!_isShooting)
            {
                StartFiring();
            }
        }
        else
        {
            if (_isShooting)
            {
                StopFiring();
            }
        }
    }

    private bool InFront()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        return angle < sightAngle / 2f;
    }

    private bool HaveLineOfSight()
    {
        if (target == null)
            return false;

        RaycastHit hit;
        Vector3 direction = (target.position - muzzlePoint.position).normalized;

        if (Physics.Raycast(muzzlePoint.position, direction, out hit, detectionRange))
        {
            Debug.Log(hit.transform.CompareTag("Player"));

            if (hit.transform.CompareTag("Player"))
            { 
                Debug.DrawRay(muzzlePoint.position, direction * hit.distance, Color.red);
                return true;
            }
            else
            {
                Debug.DrawRay(muzzlePoint.position, direction * hit.distance, Color.yellow);
            }
        }

        return false;
    }
    private void StartFiring()
    {
        beam.enabled = true;
        StartCoroutine(_shootCoroutine);
        _isShooting = true;
    }

    private void StopFiring()
    {
        beam.enabled = false;
        StopCoroutine(_shootCoroutine);
        _isShooting = false;
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            var beamEndPosition = target.position; 

            beam.SetPosition(0, muzzlePoint.position);
            beam.SetPosition(1, beamEndPosition);

            RaycastHit hit;
            Vector3 direction = (target.position - muzzlePoint.position).normalized;

            if (Physics.Raycast(muzzlePoint.position, direction, out hit, maxRange))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    DroneHealth player = hit.transform.GetComponent<DroneHealth>();
                    if (player != null)
                    {
                        player.TakeDamage(laserDamagePerSecond * Time.deltaTime);
                    }
                }
            }

            yield return null;
        }
    }
}
