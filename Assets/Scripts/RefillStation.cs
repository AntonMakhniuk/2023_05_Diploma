using UnityEngine;
using UnityEngine.UI;

public class RefillStation : BuildingObject
{
    [SerializeField] private float refillRate = 1f;
    [SerializeField] private float maxFuelCapacity = 100f; 

    private float currentFuelLevel; 
    private bool isRefilling; 

    private void Start()
    {
        currentFuelLevel = maxFuelCapacity; 
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
            Debug.Log("Player entered refill station trigger area.");

            // Show UI prompt to refill fuel
           // if (refillPromptText != null)
            {
              //  refillPromptText.gameObject.SetActive(true);
            }

            RefillFuel(other.gameObject);
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


