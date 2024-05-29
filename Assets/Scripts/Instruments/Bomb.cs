using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    private Rigidbody rb;
    private bool isFrozen = false;
    private BombContainer bombContainer;

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
            rb.velocity = rb.velocity.normalized * bombContainer.bombSpeed;
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
                Asteroid asteroid = collider.GetComponent<Asteroid>();
                if (asteroid != null)
                {
                    asteroid.Explode();
                }
            }
        }

        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
    public void SetBombContainer(BombContainer container)
    {
        bombContainer = container;
    }
}