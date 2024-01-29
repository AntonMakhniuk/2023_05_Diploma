using System.Linq;
using Production.Crafting;
using Scriptable_Object_Templates;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryWindow : MonoBehaviour
    {
        public GameObject inventoryPanel; // Reference to the UI panel
        public TextMeshProUGUI fuelGasText;
        public TextMeshProUGUI spaceOreText;
        public TextMeshProUGUI fuelText;
        public TextMeshProUGUI spaceMetalText;

        public Resource fuelGasResource; // Reference to the Fuel Gas Scriptable Object
        public Resource spaceOreResource; // Reference to the Space Ore Scriptable Object
        
        public Recipe fuelRecipe; 
        public Recipe spacemetalRecipe;
        public Button fuelRecipeButton;
        public Button spacemetalRecipeButton;

        public float fuelGasQuantity; // Quantity not in Scriptable Object
        public float spaceOreQuantity; // Quantity not in Scriptable Object

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

            if (inventoryPanel.activeSelf) {
                if (fuelGasQuantity < fuelRecipe.resources[0].quantity)
                    fuelRecipeButton.interactable = false;
                else
                    fuelRecipeButton.interactable = true;
            
                if (spaceOreQuantity < spacemetalRecipe.resources[1].quantity || fuelGasQuantity < spacemetalRecipe.resources[0].quantity)
                    spacemetalRecipeButton.interactable = false;
                else
                    spacemetalRecipeButton.interactable = true;
                
                fuelGasText.text = fuelGasResource.label + ": " +  fuelGasQuantity;
                spaceOreText.text = spaceOreResource.label + ": " + spaceOreQuantity;
            }
        }

        public void ToggleInventoryWindow()
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

                string resources = ": ";
                foreach (var resource in fuelRecipe.resources) {
                    resources += "\n" + resource.resource.label + "(" + resource.quantity + ")";
                }

                fuelText.text = fuelRecipe.label + resources;
                
                resources = ": ";
                foreach (var resource in spacemetalRecipe.resources) {
                    resources += "\n" + resource.resource.label + "(" + resource.quantity + ")";
                }

                spaceMetalText.text = spacemetalRecipe.label + resources;
            }
        }

        
        public void IncreaseFuelGasQuantity(float amount)
        {
            fuelGasQuantity += amount;
        }

        
        public void IncreaseSpaceOreQuantity(float amount)
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