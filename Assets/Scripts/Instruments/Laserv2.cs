using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserV2 : MonoBehaviour
{
    [SerializeField] private LineRenderer _beam;
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private float maxLenght;

    private void Awake()
    {
        _beam.enabled = false;
        _beam.startWidth = 0.1f;
        _beam.endWidth = 0.1f;
    }

    private void Activate()
    {
        _beam.enabled = true;
    }
    
    private void Deactivate()
    {
        _beam.enabled = false;
        _beam.SetPosition(0, _muzzlePoint.position);
        _beam.SetPosition(1, _muzzlePoint.position);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Activate();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Deactivate();
        }
    }

    private void FixedUpdate()
    {
        if (!_beam.enabled)
        {
            return;
        }

        Ray ray = new Ray(_muzzlePoint.position, _muzzlePoint.forward);
        bool cast = Physics.Raycast(ray, out RaycastHit hit, maxLenght);
        Vector3 hitPosition = cast ? hit.point : _muzzlePoint.position + _muzzlePoint.forward * maxLenght;
        _beam.SetPosition(0, _muzzlePoint.position);
        _beam.SetPosition(1,hitPosition);
        if (cast & hit.collider.CompareTag("AsteroidPoint"))
        {
            CheckAndDestroyAsteroidPoint(hit.collider);
        }
    }
    private void CheckAndDestroyAsteroidPoint(Collider collider)
    {
        if (collider.CompareTag("AsteroidPoint"))
        {
            Destroy(collider.gameObject);

            Asteroid asteroid = collider.transform.parent.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.OnAsteroidPointDestroyed();
            }
        }
    }
}
