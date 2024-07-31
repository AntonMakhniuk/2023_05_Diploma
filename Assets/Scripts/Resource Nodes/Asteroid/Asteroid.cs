using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(RandomPointSelector))]
public class Asteroid : MonoBehaviour, IDestructible
{
   [SerializeField] private float MaxHP { get;  set; } = 1000f; // Example max HP
    private float CurrentHP { get;   set; }
  //  public IInstrument CurrentInstrument { get; set; }

    private int asteroidPointsCount = 0;
    [SerializeField] private GameObject spaceOre;
    [SerializeField] private bool shatter = false;
    private bool _shattered = false;
    private RandomPointSelector randomPointSelector;
    
    [SerializeField] private GameObject _wholeAsteroid;
    private Rigidbody _wholeAsteroidRb;
    private Quaternion _asteroidRotationOffset;
    [SerializeField] private GameObject _fracturedAsteroid;
    
    [SerializeField] private float _explosionRadius = 1f;
    [SerializeField] private float _explosionPower = 1f;

    private void Start()
    {
        _wholeAsteroidRb = _wholeAsteroid.GetComponent<Rigidbody>();
        _asteroidRotationOffset = _wholeAsteroid.transform.localRotation;

        randomPointSelector = GetComponent<RandomPointSelector>();
        CurrentHP = MaxHP;
        // Count the initial number of asteroid points
        asteroidPointsCount = randomPointSelector.GetAsteroidPoints().Count;
    }

    private void Update()
    {
        if (shatter && !_shattered)
            ShatterAsteroid();
    }

    float IDestructible.MaxHP
    {
        get => MaxHP;
        set => MaxHP = value;
    }

    float IDestructible.CurrentHP
    {
        get => MaxHP;
        set => MaxHP = value;
    }


    public void OnLaserInteraction(float damage)
    {
        Debug.Log("Asteroid can not take damage from laser");
    }

    public void OnDrillInteraction(float damage)
    {
        // TakeDamage(damage);
    }


    public void OnCutterInteraction(float damage)
    {
        // TakeDamage(damage);
    }

    public void OnExplosivesInteraction(float damage)
    {
        // TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            InitiateDestroy();
        }
    }

    public void InitiateDestroy()
    {
        ShatterAsteroid();
    }

    public void OnAsteroidPointDestroyed()
    {
        // Decrement the count of remaining asteroid points
        asteroidPointsCount--;

        // Take damage based on the number of asteroid points
        TakeDamage(MaxHP / 3);

        // Check if all points are destroyed
        if (asteroidPointsCount <= 0)
        {
            // Shatter the entire asteroid into random pieces
            ShatterAsteroid();
        }
    }

    public void ShatterAsteroid()
    {
        _shattered = true;
        _fracturedAsteroid.transform.position = _wholeAsteroid.transform.position;
        _fracturedAsteroid.transform.rotation = _wholeAsteroid.transform.localRotation * Quaternion.Inverse(_asteroidRotationOffset);
        Vector3 explosionPos = _fracturedAsteroid.transform.position;
        var asteroidVelocity = _wholeAsteroidRb.velocity;
        
        _wholeAsteroid.SetActive(false);
        _fracturedAsteroid.SetActive(true);
        
        // Spawn ore at center 
        Vector3 asteroidPosition = _wholeAsteroid.transform.position;
        int oreNumber = Random.Range(1, 6);
        while (oreNumber > 0)
        {
            SpawnOre(asteroidPosition);
            asteroidPosition[Random.Range(0, 3)] += Random.Range(0.4f, 0.9f);
            oreNumber--;
        }
        
        Collider[] colliders = Physics.OverlapSphere(explosionPos, _explosionRadius);
        
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
        
            if (rb == null)
                return;

            rb.velocity = asteroidVelocity;
            rb.AddExplosionForce(_explosionPower, explosionPos, _explosionRadius, 0);
            rb.transform.parent = null;
        }
        
        Destroy(_wholeAsteroid);
        Destroy(this.gameObject);
    }

    private void SpawnOre(Vector3 position)
    {
        Instantiate(spaceOre, position, Quaternion.Euler(Random.Range(200, 360), Random.Range(200, 360), Random.Range(200, 360)));
    }

}
