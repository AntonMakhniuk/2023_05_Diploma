using Assets.Scripts.Instruments;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Instrument
{
    [SerializeField] private Canvas crosshairCanvas;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private float laserRange = 100f;
    [SerializeField] private LayerMask asteroidLayer;
    [SerializeField] private Color validColor = Color.green;
    [SerializeField] private Color invalidColor = Color.red;

    private Camera mainCamera;

    private bool isLaserActive = false;

    protected override void Awake()
    {
        base.Awake();

        mainCamera = Camera.main;

        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material.color = invalidColor;

        ToggleBarrel(false);
        crosshairCanvas.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToggleLaser();
        }

        if (isLaserActive && Input.GetMouseButton(0))
        {
            UpdateCrosshairPosition();

            Ray ray = mainCamera.ScreenPointToRay(crosshairCanvas.transform.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, laserRange, asteroidLayer))
            {
                DrawLaser(barrelTransform.position, hit.point);
                UpdateLaserColor(hit);
                CheckAndDestroyAsteroidPoint(hit.collider);
            }
            else
            {
                Vector3 endPos = ray.GetPoint(laserRange);
                DrawLaser(barrelTransform.position, endPos);
                crosshairCanvas.GetComponent<Canvas>().enabled = false;
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
        
        RotateBarrel();
    }

    private void ToggleLaser()
    {
        isLaserActive = !isLaserActive;
        crosshairCanvas.enabled = isLaserActive;
        base.Toggle();
        ToggleBarrel(isLaserActive);
    }

    private void ToggleBarrel(bool active)
    {
        barrelTransform.gameObject.SetActive(active);
    }

    private void UpdateCrosshairPosition()
    {
        Vector3 crosshairPosition = mainCamera.WorldToScreenPoint(barrelTransform.position + Vector3.up * 2f); 
        crosshairCanvas.transform.position = crosshairPosition;
    }

    private void UpdateLaserColor(RaycastHit hit)
    {
        lineRenderer.material.color = IsTargetClear(hit) ? validColor : invalidColor;
    }

    private void DrawLaser(Vector3 startPos, Vector3 endPos)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    private bool IsTargetClear(RaycastHit hit)
    {
        return hit.collider.CompareTag("AsteroidPoint");
    }

    private void CheckAndDestroyAsteroidPoint(Collider collider)
    {
        if (collider.CompareTag("AsteroidPoint"))
        {
            Destroy(collider.gameObject);

            Asteroid asteroid = collider.transform.parent.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.OnAsteroidPointDestroyed();
            }
        }
    }

    private void RotateBarrel()
    {
        if (isLaserActive)
        {
            Vector3 cameraRotation = mainCamera.transform.localRotation.eulerAngles;
            Quaternion targetRotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0f);
            barrelTransform.rotation = Quaternion.Lerp(barrelTransform.rotation, targetRotation, Time.deltaTime);
        }
    }
}

