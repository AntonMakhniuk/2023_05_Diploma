using Assets.Scripts.Instruments;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Drill : Instrument
{
    [SerializeField] private float drillingTime = 2f;
    [SerializeField] private Transform drillModel;
    [SerializeField] private GameObject sliderCanvasPrefab;
    private Slider timerSlider;

    private bool isDrilling = false;
    private bool isDrillingFinished = false;
    private float currentDrillingTime = 0f;

    protected override void Awake()
    {
        base.Awake();
        // Ensure that the sliderCanvasPrefab reference is not null
        if (sliderCanvasPrefab == null)
        {
            Debug.LogError("Slider Canvas Prefab reference is not set.");
            enabled = false; // Disable the script to prevent errors
            return;
        }

        // Instantiate the Slider Canvas as a child of the player
        GameObject sliderCanvasInstance = Instantiate(sliderCanvasPrefab, transform);
        // Find the Slider component in the instantiated canvas
        timerSlider = sliderCanvasInstance.GetComponentInChildren<Slider>();
        if (timerSlider != null)
        {
            // Hide the slider initially
            timerSlider.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Slider component not found in the Slider Canvas prefab.");
        }
    }

    private void Update()
    {
        // Check for left mouse button input
        if (Input.GetMouseButtonDown(0))
        {
            // Start drilling if the drill is the active tool
            if (isActiveTool)
            {
                StartDrilling();
            }
        }

        // Stop drilling if left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            StopDrilling();
        }
    }

    private void StartDrilling()
    {
        // Show the timer UI
        timerSlider.gameObject.SetActive(true);
        isDrilling = true;
        // Start the drilling coroutine
        StartCoroutine(DrillingCoroutine());
    }

    private void StopDrilling()
    {
        // Hide the timer UI
        timerSlider.gameObject.SetActive(false);
        isDrilling = false;
        // Reset the drilling time
        currentDrillingTime = 0f;
    }

    private IEnumerator DrillingCoroutine()
    {
        while (isDrilling)
        {
            // Increment the drilling time
            currentDrillingTime += Time.deltaTime;
            // Update the timer UI
            timerSlider.value = currentDrillingTime / drillingTime;
            // If drilling time exceeds the total drilling time, break the asteroid
            if (currentDrillingTime >= drillingTime)
            {
                isDrillingFinished = true;
                break;
            }
            yield return null;
        }
    }

    private void LateUpdate()
    {
        // Rotate the drill model while drilling
        if (isDrilling)
        {
            RotateDrill();
        }
    }

    private void RotateDrill()
    {
        // Rotate the drill model around its local axis
        if (drillModel != null)
        {
            drillModel.Rotate(Vector3.forward, Time.deltaTime * 360f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is an asteroid
        if (other.CompareTag("Asteroid"))
        {
            // Start drilling when the drill comes into contact with the asteroid
            StartDrilling();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the collided object is an asteroid
        if (other.CompareTag("Asteroid"))
        {
            // If drilling finished, destroy the asteroid
            if (isDrillingFinished)
            {
                Destroy(other.gameObject);
                isDrillingFinished = false; // Reset flag
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the collided object is an asteroid
        if (other.CompareTag("Asteroid"))
        {
            // Stop drilling when the drill moves away from the asteroid
            StopDrilling();
        }
    }
}
