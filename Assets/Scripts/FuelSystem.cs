using UnityEngine;
using UnityEngine.UI;

public class FuelSystem : MonoBehaviour
{
    [SerializeField] private float maxFuelCapacity = 100f;
    private float currentFuelLevel;
    public Text fuelText;
    public Slider fuelSlider;
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

    public float GetLowFuelThreshold()
    {
        // return currentFuelLevel > maxFuelCapacity * lowFuelThreshold ? true : false;
        return lowFuelThreshold;
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
        // Update the value of the fuel slider
        fuelSlider.value = currentFuelLevel;
        //fuelText.text = currentFuelLevel <= 0 ? "Out of Fuel!" : "Fuel Level: " + Mathf.Round(currentFuelLevel) + "%";

        // Optionally, change the color of the slider handle based on fuel level
        fuelText.text = currentFuelLevel <= maxFuelCapacity * lowFuelThreshold ? "Low Fuel Level: " + Mathf.Round(currentFuelLevel) + "%" : "Fuel Level: " + Mathf.Round(currentFuelLevel) + "%";
        fuelText.text = currentFuelLevel <= 1 ? "Out of Fuel!" : "Fuel Level: " + Mathf.Round(currentFuelLevel) + "%";
        fuelText.color = currentFuelLevel <= maxFuelCapacity * lowFuelThreshold ? lowFuelColor : Color.white;
        fuelSlider.fillRect.GetComponent<Image>().color = currentFuelLevel <= maxFuelCapacity * lowFuelThreshold ? lowFuelColor : Color.green;
    }
}
