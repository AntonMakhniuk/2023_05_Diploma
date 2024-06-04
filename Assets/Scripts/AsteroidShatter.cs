using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidShatter : MonoBehaviour
{
    [SerializeField] private GameObject _wholeAsteroid;
    private Rigidbody _wholeAsteroidRb;
    private Quaternion _asteroidRotationOffset;
    [SerializeField] private GameObject _fracturedAsteroid;
    
    [SerializeField] private bool _shatter = false;
    private bool _shattered = false;
    [SerializeField] private float _explosionRadius = 1f;
    [SerializeField] private float _explosionPower = 1f;

    private void Awake()
    {
        _wholeAsteroidRb = _wholeAsteroid.GetComponent<Rigidbody>();
        _asteroidRotationOffset = _wholeAsteroid.transform.localRotation;
    }

    private void Update()
    {
        if (_shatter && !_shattered)
            Shatter();
    }

    private void Shatter()
    {
        _shattered = true;
        _fracturedAsteroid.transform.position = _wholeAsteroid.transform.position;
        _fracturedAsteroid.transform.rotation = _wholeAsteroid.transform.localRotation * Quaternion.Inverse(_asteroidRotationOffset);
        Vector3 explosionPos = _fracturedAsteroid.transform.position;
        var asteroidVelocity = _wholeAsteroidRb.velocity;
        
        _wholeAsteroid.SetActive(false);
        _fracturedAsteroid.SetActive(true);
        
        Collider[] colliders = Physics.OverlapSphere(explosionPos, _explosionRadius);
        
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
        
            if (rb == null)
                return;

            rb.velocity = asteroidVelocity;
            rb.AddExplosionForce(_explosionPower, explosionPos, _explosionRadius, 0);
        }
    }
}
