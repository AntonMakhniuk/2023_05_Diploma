/*
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
        if (sliderCanvasPrefab == null)
        {
            enabled = false; 
            return;
        }
        
        GameObject sliderCanvasInstance = Instantiate(sliderCanvasPrefab, transform);
        timerSlider = sliderCanvasInstance.GetComponentInChildren<Slider>();
        if (timerSlider != null)
        {
            timerSlider.gameObject.SetActive(false);
        }
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isActiveTool)
            {
                StartDrilling();
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            StopDrilling();
        }
    }

    private void StartDrilling()
    {
        timerSlider.gameObject.SetActive(true);
        isDrilling = true;
        StartCoroutine(DrillingCoroutine());
    }

    private void StopDrilling()
    {
        timerSlider.gameObject.SetActive(false);
        isDrilling = false;
        currentDrillingTime = 0f;
    }

    private IEnumerator DrillingCoroutine()
    {
        while (isDrilling)
        {
            currentDrillingTime += Time.deltaTime;
            timerSlider.value = currentDrillingTime / drillingTime;
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
        if (isDrilling)
        {
            RotateDrill();
        }
    }

    private void RotateDrill()
    {
        drillModel.Rotate(Vector3.forward, Time.deltaTime * 360f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
            StartDrilling();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
            if (isDrillingFinished)
            {
                Destroy(other.gameObject);
                isDrillingFinished = false; 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
            StopDrilling();
        }
    }
}
*/
