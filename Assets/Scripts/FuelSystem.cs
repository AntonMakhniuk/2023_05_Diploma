using UnityEngine;
using UnityEngine.UI;

public class FuelSystem : MonoBehaviour
{
    private MovementInputSystem movementInputSystem;

    [SerializeField] private float maxFuelCapacity = 100f;
    private float currentFuelLevel;
    public Text fuelText;
    public Color lowFuelColor = Color.red;
    private float lowFuelThreshold = 0.33f;

    private void Awake()
    {
        currentFuelLevel = maxFuelCapacity;
        UpdateFuelIndicator();
    }

    public void AddFuel(float amount)
    {
        currentFuelLevel = Mathf.Min(currentFuelLevel + amount, maxFuelCapacity);
        UpdateFuelIndicator();
    }

    public void ConsumeFuel(float amount)
    {
        if (currentFuelLevel >= amount)
        {
            currentFuelLevel -= amount;
            UpdateFuelIndicator();
        } 
    }

    public bool GetLowFuelThreshold()
    {
        return currentFuelLevel > maxFuelCapacity * lowFuelThreshold ? true : false ;
    }

    public float GetMaxFuelCapacity()
    {
        return maxFuelCapacity;
    }

    public float GetCurrentFuelLevel()
    {
        return currentFuelLevel;
    }

    private void UpdateFuelIndicator()
    {
        fuelText.text = "Fuel Level: " + Mathf.Round(currentFuelLevel) + "%";
        fuelText.color = currentFuelLevel <= maxFuelCapacity * lowFuelThreshold ? lowFuelColor : Color.white;
    }
}
