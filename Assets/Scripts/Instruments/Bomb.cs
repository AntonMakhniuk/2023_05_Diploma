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
        // If the bomb is not frozen, keep its velocity constant
        if (!isFrozen)
        {
            rb.velocity = rb.velocity.normalized * GetComponentInParent<BombContainer>().bombSpeed;
        }
    }

    void FreezeBomb()
    {
        rb.velocity = Vector3.zero;
        isFrozen = true;

        // Show bomb's range
        ShowBombRange();
    }

    void ShowBombRange()
    {
        // Draw bomb range using Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public void Detonate()
    {
        // Detonate the bomb
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
