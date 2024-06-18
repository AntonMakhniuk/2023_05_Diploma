using Building.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Building.Structures
{
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
            // Check for null components
            if (fuelStoreWindow == null) throw new MissingComponentException("fuelStoreWindow is not assigned.");
            if (refillPromptText == null) throw new MissingComponentException("refillPromptText is not assigned.");
            if (fuelSlider == null) throw new MissingComponentException("fuelSlider is not assigned.");
            if (sliderValueText == null) throw new MissingComponentException("sliderValueText is not assigned.");
            if (acceptButton == null) throw new MissingComponentException("acceptButton is not assigned.");

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
            if (!other.CompareTag("Player")) return;

            if (!isRefilling && currentFuelLevel > 0)
            {
                refillPromptText.gameObject.SetActive(true);
                RefillFuel(other.gameObject);
            }

            fuelSlider.gameObject.SetActive(true);
            acceptButton.gameObject.SetActive(true);

            acceptButton.onClick.AddListener(() => StoreFuel(other.gameObject, fuelSlider.value));

            Debug.Log("Player entered station trigger area.");

            fuelStoreWindow.SetActive(true);

            FuelSystem fuelSystem = other.GetComponent<FuelSystem>();
            if (fuelSystem != null)
            {
                fuelSlider.maxValue = fuelSystem.GetCurrentFuelLevel();
            }

            fuelSlider.onValueChanged.AddListener((value) => UpdateSliderValueText());
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player exited station trigger area.");

                fuelStoreWindow.SetActive(false);

                fuelSlider.onValueChanged.RemoveAllListeners();
                acceptButton.onClick.RemoveAllListeners();
            }
        }

        private void UpdateSliderValueText()
        {
            sliderValueText.text = "Selected amount: " + fuelSlider.value.ToString("F1");
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
}
