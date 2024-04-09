using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float freezeTime = 3f; 
    public float explosionRadius = 5f;

    private Rigidbody rb;
    private bool isFrozen = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("FreezeBomb", freezeTime);
    }

    void Update()
    {
       
        if (!isFrozen)
        {
            rb.velocity = rb.velocity.normalized * GetComponentInParent<BombContainer>().bombSpeed;
        }
    }

    void FreezeBomb()
    {
        rb.velocity = Vector3.zero;
        isFrozen = true;
        
        ShowBombRange();
    }

    void ShowBombRange()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public void Detonate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Asteroid"))
            {
                Destroy(collider.gameObject);
            }
        }
        Destroy(gameObject);
    }
}
