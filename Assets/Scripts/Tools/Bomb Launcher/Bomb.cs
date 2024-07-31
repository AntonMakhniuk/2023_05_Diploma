using System.Collections;
using System.Collections.Generic;
using Instruments.Bomb_Launcher;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    private Rigidbody rb;
    private bool isFrozen = false;
    private BombLauncher _bombLauncher;

    public float freezeTime = 3f;
    public float explosionRadius = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("FreezeBomb", freezeTime);
    }

    void Update()
    {
       
        if (!isFrozen)
        {
            rb.velocity = rb.velocity.normalized * _bombLauncher.bombSpeed;
        }
        
    }

    void FreezeBomb()
    {
        rb.velocity = Vector3.zero;
        isFrozen = true;
    }
    
    public void Detonate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Asteroid"))
            {
                Asteroid asteroid = collider.GetComponentInParent<Asteroid>();
                if (asteroid != null)
                {
                    asteroid.ShatterAsteroid();
                }
            }
        }

        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
    public void SetBombContainer(BombLauncher launcher)
    {
        _bombLauncher = launcher;
    }
}