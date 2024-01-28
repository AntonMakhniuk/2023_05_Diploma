using UnityEngine;

public class DrillBehaviour : MonoBehaviour
{
    private Renderer drillRenderer;
    private Collider drillCollider;

    private void Start()
    {
        // Get the renderer and collider components
        drillRenderer = GetComponent<Renderer>();
        drillCollider = GetComponent<Collider>();

        // Hide the drill by default
        HideDrill();
    }

    private void Update()
    {
        // Toggle drill visibility on the "2" key
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToggleDrillVisibility();
        }
    }

    private void ToggleDrillVisibility()
    {
        // Toggle the visibility of the drill by enabling/disabling its renderer and collider
        if (drillRenderer != null && drillCollider != null)
        {
            if (drillRenderer.enabled)
            {
                HideDrill();
            }
            else
            {
                ShowDrill();
            }
        }
    }

    private void HideDrill()
    {
        // Hide the drill by disabling its renderer and collider
        if (drillRenderer != null && drillCollider != null)
        {
            drillRenderer.enabled = false;
            drillCollider.enabled = false;
        }
    }

    private void ShowDrill()
    {
        // Show the drill by enabling its renderer and collider
        if (drillRenderer != null && drillCollider != null)
        {
            drillRenderer.enabled = true;
            drillCollider.enabled = true;
        }
    }

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
