using Scriptable_Object_Templates;
using UnityEngine;
using TMPro;

public class InventoryWindow : MonoBehaviour
{
    public GameObject inventoryPanel; // Reference to the UI panel
    public TextMeshProUGUI  fuelGasText;
    public TextMeshProUGUI  spaceOreText;

    public Resource fuelGasObject; // Reference to the Fuel Gas Scriptable Object
    public Resource spaceOreObject; // Reference to the Space Ore Scriptable Object

    void Start()
    {
        // Ensure the inventory panel is initially hidden
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryWindow();
        }
    }

    void ToggleInventoryWindow()
    {
        // Toggle the visibility of the inventory panel
        if (inventoryPanel != null)
        {
            bool isPanelActive = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(!isPanelActive);
        }

        // Update UI Text elements
        if (fuelGasObject != null && spaceOreObject != null)
        {
            fuelGasText.text = "Fuel Gas: " + fuelGasObject.label;
            spaceOreText.text = "Space Ore: " + spaceOreObject.label;
        }
    }
}