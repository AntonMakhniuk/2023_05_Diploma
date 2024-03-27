using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
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
        // If the bomb is not frozen, keep its velocity constant
        if (!isFrozen)
        {
            rb.velocity = rb.velocity.normalized * bombContainer.bombSpeed;
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
                Asteroid asteroid = collider.GetComponent<Asteroid>();
                if (asteroid != null)
                {
                    asteroid.Explode();
                }
            }
        }
        Destroy(gameObject);
    }

    // Method to set the reference to the BombContainer script
    public void SetBombContainer(BombContainer container)
    {
        bombContainer = container;
    }
}