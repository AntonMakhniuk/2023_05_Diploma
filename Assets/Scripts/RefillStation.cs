using UnityEngine;
using UnityEngine.UI;

public class RefillStation : MonoBehaviour
{
    [SerializeField] private float refillRate = 1f; // Rate of fuel generation per second
    [SerializeField] private float maxFuelCapacity = 100f; // Maximum fuel capacity of the station
   // [SerializeField] private Text refillPromptText; // Reference to the UI Text for refill prompt

    private float currentFuelLevel; // Current fuel level of the station
    private bool isRefilling; // Flag to track if the station is currently refilling a player

    private void Start()
    {
        currentFuelLevel = maxFuelCapacity; // Start with full fuel
    }

    private void Update()
    {
        // If the station is not currently refilling a player and it has fuel to generate
        if (!isRefilling && currentFuelLevel < maxFuelCapacity)
        {
            // Generate fuel over time
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

            // Refill player's fuel
            RefillFuel(other.gameObject);
        }
    }

  //  private void OnTriggerExit(Collider other)
    //{
      //  if (other.CompareTag("Player"))
        //{
            // Hide UI prompt when player leaves the refill station trigger area
          //  if (refillPromptText != null)
            //{
              //  refillPromptText.gameObject.SetActive(false);
            //}
        //}
    //}

    private void RefillFuel(GameObject player)
    {
        FuelSystem fuelSystem = player.GetComponent<FuelSystem>();
        if (fuelSystem != null)
        {
            // Calculate the amount of fuel to refill
            float refillAmount = Mathf.Min(fuelSystem.GetMaxFuelCapacity() - fuelSystem.GetCurrentFuelLevel(), currentFuelLevel);

            // Refill fuel
            fuelSystem.AddFuel(refillAmount);
            currentFuelLevel -= refillAmount;

            // Mark the station as currently refilling
            isRefilling = true;

            // Reset the refilling flag after a short delay (to simulate refilling time)
            Invoke("ResetRefillingFlag", 1.0f);

            // Optionally, play a sound effect or show visual feedback indicating fuel refill
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


