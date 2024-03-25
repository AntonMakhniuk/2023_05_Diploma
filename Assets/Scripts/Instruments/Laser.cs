using Assets.Scripts.Instruments;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Instrument
{
    [SerializeField] private Canvas crosshairCanvas;
    [SerializeField] private float laserRange = 100f;
    [SerializeField] private LayerMask asteroidLayer;
    [SerializeField] private Color validColor = Color.green;
    [SerializeField] private Color invalidColor = Color.red;

    private LineRenderer lineRenderer;
    private Transform barrelTransform;

    private bool isLaserActive = false;
    private Camera mainCamera;

    void Start()
    {
        // Create the LineRenderer as a child object of the Laser prefab
        GameObject lineObject = new GameObject("LineRenderer");
        lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material.color = Color.red;
        lineObject.transform.SetParent(transform);
        lineObject.transform.localPosition = Vector3.zero;
        lineRenderer.enabled = false;

        mainCamera = Camera.main;
        barrelTransform = transform.Find("Barrel (Laser)");
        
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.Alpha4))
    {
        isLaserActive = !isLaserActive;
        crosshairCanvas.enabled = isLaserActive;
    }

    if (isLaserActive && Input.GetMouseButton(0))
    {
        // Update crosshair position
        crosshairCanvas.transform.position = mainCamera.WorldToScreenPoint(transform.position + transform.up * 5f);

        // Rotate laser barrel according to camera rotation
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0f; // Keep the laser parallel to the ground
        barrelTransform.forward = cameraForward.normalized;

        // Raycast from camera to crosshair position
        Ray ray = mainCamera.ScreenPointToRay(crosshairCanvas.transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, laserRange))
        {
            // Check if the hit object is an asteroid point
            if (hit.collider.CompareTag("AsteroidPoint"))
            {
                // Destroy the collided asteroid point
                Destroy(hit.collider.gameObject);

                // Find the Asteroid script on the parent asteroid
                Asteroid asteroid = hit.collider.transform.parent.GetComponent<Asteroid>();

                // Notify the attached asteroid about the destruction
                if (asteroid != null)
                {
                    asteroid.OnAsteroidPointDestroyed();
                }
            }

            // Draw laser line
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, barrelTransform.position);
            lineRenderer.SetPosition(1, hit.point);

            // Change color of crosshair based on whether laser can shoot or not
            crosshairCanvas.GetComponent<Canvas>().enabled = IsTargetClear(hit);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
    else
    {
        lineRenderer.enabled = false;
        crosshairCanvas.GetComponent<Canvas>().enabled = false;
    }
}

    bool IsTargetClear(RaycastHit hit)
    {
        return !Physics.Raycast(barrelTransform.position, (hit.point - barrelTransform.position), Vector3.Distance(barrelTransform.position, hit.point), asteroidLayer);
    }
}

