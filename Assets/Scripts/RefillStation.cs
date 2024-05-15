using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RefillStation : BuildingObject
{
    public GameObject fuelStoreWindow;

    public TextMeshProUGUI refillPromptText;
    public Slider fuelSlider;
    public TextMeshProUGUI sliderValueText;
    public Button acceptButton;

    [SerializeField] private float refillRate = 1f;
    [SerializeField] private float maxFuelCapacity = 100f;

    private float currentFuelLevel;
    private bool isRefilling;

    private void Start()
    {
        currentFuelLevel = maxFuelCapacity / 2;
    }

    private void Update()
    {
        if (!isRefilling && currentFuelLevel < maxFuelCapacity)
        {
            currentFuelLevel = Mathf.Min(currentFuelLevel + refillRate * Time.deltaTime, maxFuelCapacity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isRefilling && currentFuelLevel > 0)
        {
            // Show UI prompt to refill fuel
            if (refillPromptText != null)
            {
                refillPromptText.gameObject.SetActive(true);
            }

            RefillFuel(other.gameObject);
        }

        // Show UI slider and accept button
        if (fuelSlider != null && acceptButton != null)
        {
            fuelSlider.gameObject.SetActive(true);
            acceptButton.gameObject.SetActive(true);

            // Add listener to accept button
            acceptButton.onClick.AddListener(() => StoreFuel(other.gameObject, fuelSlider.value));
        }

        Debug.Log("Player entered station trigger area.");

        if (fuelStoreWindow != null)
        {
            fuelStoreWindow.SetActive(true);
        }

        // Set max value of the slider to the player's current fuel amount
        if (other.CompareTag("Player"))
        {
            FuelSystem fuelSystem = other.GetComponent<FuelSystem>();
            if (fuelSystem != null)
            {
                fuelSlider.maxValue = fuelSystem.GetCurrentFuelLevel();
            }
        }

        // Add listener to slider value change
        fuelSlider.onValueChanged.AddListener(delegate { UpdateSliderValueText(); });
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited station trigger area.");

            if (fuelStoreWindow != null)
            {
                fuelStoreWindow.SetActive(false);
            }

            // Remove listeners to prevent multiple listeners being added
            fuelSlider.onValueChanged.RemoveAllListeners();
            acceptButton.onClick.RemoveAllListeners();
        }
    }

    private void UpdateSliderValueText()
    {
        if (sliderValueText != null && fuelSlider != null)
        {
            sliderValueText.text = "Selected amount: " + fuelSlider.value.ToString("F1");
        }
    }

    private void StoreFuel(GameObject player, float storeAmount)
    {
        FuelSystem fuelSystem = player.GetComponent<FuelSystem>();
        if (fuelSystem != null)
        {
            float remainingCapacity = maxFuelCapacity - currentFuelLevel;
            float actualStoreAmount = Mathf.Min(remainingCapacity, storeAmount);

            fuelSystem.ConsumeFuel(actualStoreAmount);
            currentFuelLevel += actualStoreAmount;

            isRefilling = true;

            Invoke("ResetRefillingFlag", 1.0f);

            Debug.Log("Fuel stored: " + actualStoreAmount + " units");
            Debug.Log("Fuel available: " + currentFuelLevel + " units");
        }
        else
        {
            Debug.LogWarning("No FuelSystem component found on the player object.");
        }
    }

    private void RefillFuel(GameObject player)
    {
        FuelSystem fuelSystem = player.GetComponent<FuelSystem>();
        if (fuelSystem != null)
        {
            float refillAmount = Mathf.Min(fuelSystem.GetMaxFuelCapacity() - fuelSystem.GetCurrentFuelLevel(), currentFuelLevel);

            fuelSystem.AddFuel(refillAmount);
            currentFuelLevel -= refillAmount;

            isRefilling = true;

            Invoke("ResetRefillingFlag", 1.0f);

            Debug.Log("Fuel refilled by: " + refillAmount + " units");
        }
        else
        {
            Debug.LogWarning("No FuelSystem component found on the player object.");
        }
    }

    private void ResetRefillingFlag()
    {
        isRefilling = false;
    }
}
