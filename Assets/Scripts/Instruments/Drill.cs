using Assets.Scripts.Instruments;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drill : Instrument
{
    [SerializeField] private float drillingTime = 5f;
    [SerializeField] private Transform drillModel;
    [SerializeField] private GameObject asteroidGameObject;

    private bool isDrilling = false;
    private float currentDrillingTime = 0f;
    private Coroutine drillingCoroutine;
    private Slider timerSlider;

    protected override void Awake()
    {
        base.Awake();
        // Create the timer slider
        CreateTimerSlider();
    }

    private void CreateTimerSlider()
    {
        // Create a canvas if one doesn't exist
        GameObject canvasObject = new GameObject("SliderCanvas");
        canvasObject.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();
        canvasObject.tag = "Slider";
        canvasObject.layer = 5;

        // Create the slider GameObject
        GameObject sliderObject = new GameObject("TimerSlider");
        sliderObject.transform.SetParent(canvasObject.transform);

        // Add Slider component
        timerSlider = sliderObject.AddComponent<Slider>();

        // Set slider properties
        timerSlider.minValue = 0f;
        timerSlider.maxValue = 1f;
        timerSlider.value = 0f;

        // Set slider position and size
        RectTransform rectTransform = timerSlider.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200f, 20f);
        rectTransform.anchoredPosition = new Vector2(0f, 0f);

        // Add slider background
        GameObject background = new GameObject("Background");
        background.transform.SetParent(sliderObject.transform);
        Image backgroundImage = background.AddComponent<Image>();
        backgroundImage.color = Color.gray;

        // Add slider fill area
        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(sliderObject.transform);
        RectTransform fillAreaTransform = fillArea.AddComponent<RectTransform>();
        fillAreaTransform.anchorMin = new Vector2(0f, 0f);
        fillAreaTransform.anchorMax = new Vector2(1f, 1f);
        fillAreaTransform.sizeDelta = Vector2.zero;

        // Add fill
        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(fillArea.transform);
        Image fillImage = fill.AddComponent<Image>();
        fillImage.color = Color.green;

        // Hide the timer slider initially
        timerSlider.gameObject.SetActive(false);
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
        drillingCoroutine = StartCoroutine(DrillingCoroutine());
    }

    private void StopDrilling()
    {
        // Hide the timer UI
        timerSlider.gameObject.SetActive(false);
        isDrilling = false;
        // Reset the drilling time
        currentDrillingTime = 0f;
        // Stop the drilling coroutine if it's running
        if (drillingCoroutine != null)
        {
            StopCoroutine(drillingCoroutine);
        }
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
                BreakAsteroid();
                break;
            }
            yield return null;
        }
    }

    private void BreakAsteroid()
    {
        // Destroy the asteroid
        Destroy(asteroidGameObject);
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
