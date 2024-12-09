using System.Collections;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [Header("Blast Settings")]
    [SerializeField] private float laserDamage = 10f;
    [SerializeField] private float blastInterval = 1.0f; 
    [SerializeField] private GameObject blastPrefab;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private Transform target;

    [Header("Detection Settings")]
    [SerializeField] private float sightAngle = 90f;
    [SerializeField] private float detectionRange = 50f;

    private bool _isShooting;
    private float _lastBlastTime; 

    private void Update()
    {
        if (target == null) return;

        if (InFront() && HaveLineOfSight())
        {
            if (Time.time >= _lastBlastTime + blastInterval)
            {
                ShootBlast();
                _lastBlastTime = Time.time; 
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
        RaycastHit hit;
        Vector3 direction = (target.position - muzzlePoint.position).normalized;

        if (Physics.Raycast(muzzlePoint.position, direction, out hit, detectionRange))
        {
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

    private void ShootBlast()
    {
        if (target == null) return;

        Vector3 spawnPosition = muzzlePoint.position + muzzlePoint.forward / 2f - muzzlePoint.up / 2f;

        Vector3 targetAdjustedPosition = new Vector3(target.position.x, target.position.y - 0.8f, target.position.z);
        Vector3 direction = (targetAdjustedPosition - spawnPosition).normalized;

        Debug.DrawRay(spawnPosition, direction * detectionRange, Color.red, 2f);

        if (blastPrefab != null)
        {
            GameObject blast = Instantiate(blastPrefab, spawnPosition, Quaternion.LookRotation(direction));
            Debug.Log("Blast fired!");

            Blast blastScript = blast.GetComponent<Blast>();
            if (blastScript != null)
            {
                blastScript.SetDamage(laserDamage); 
            }
        }
    }

}
