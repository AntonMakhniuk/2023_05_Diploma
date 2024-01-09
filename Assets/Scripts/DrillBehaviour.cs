using UnityEngine;

public class DrillBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the trigger collider is an object tagged as "AsteroidPoint"
        if (other.CompareTag("AsteroidPoint"))
        {
            // Destroy the collided asteroid point
            Destroy(other.gameObject);

            // Find the Asteroid script on the parent asteroid
            Asteroid asteroid = other.transform.parent.GetComponent<Asteroid>();

            // Notify the attached asteroid about the destruction
            if (asteroid != null)
            {
                asteroid.OnAsteroidPointDestroyed();
            }
        }
    }
}
