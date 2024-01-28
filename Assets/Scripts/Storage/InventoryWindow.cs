using Scriptable_Object_Templates;
using UnityEngine;
using TMPro;

public class InventoryWindow : MonoBehaviour
    {
        public GameObject inventoryPanel; // Reference to the UI panel
        public TextMeshProUGUI fuelGasText;
        public TextMeshProUGUI spaceOreText;

        public Resource fuelGasResource; // Reference to the Fuel Gas Scriptable Object
        public Resource spaceOreResource; // Reference to the Space Ore Scriptable Object

        public int fuelGasQuantity; // Quantity not in Scriptable Object
        public int spaceOreQuantity; // Quantity not in Scriptable Object

        void Start()
        {
            // Ensure the inventory panel is initially hidden
            if (inventoryPanel != null)
            {
                inventoryPanel.SetActive(false);
            }

            // Initialize quantities
            fuelGasQuantity = 0;
            spaceOreQuantity = 0;
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

            // Update TextMeshPro elements with Scriptable Objects and calculated quantities
            if (fuelGasResource != null && spaceOreResource != null)
            {
                fuelGasText.text = fuelGasResource.label + ": " +  fuelGasQuantity;
                spaceOreText.text = spaceOreResource.label + ": " + spaceOreQuantity;
            }
        }

        
        public void IncreaseFuelGasQuantity(int amount)
        {
            fuelGasQuantity += amount;
        }

        
        public void IncreaseSpaceOreQuantity(int amount)
        {
            spaceOreQuantity += amount;
        }
        public void DecreaseFuelGasQuantity(int amount)
        {
            fuelGasQuantity -= amount;
        }

        
        public void DecreaseSpaceOreQuantity(int amount)
        {
            spaceOreQuantity -= amount;
        }
    }