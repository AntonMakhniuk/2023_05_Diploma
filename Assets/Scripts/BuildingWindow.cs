using UnityEngine;
using UnityEngine.UI;

public class BuildingWindow : MonoBehaviour
{
    public GameObject buildingWindow; // Reference to the building window Panel
    public GameObject teleporterPrefab; // Reference to the teleporter prefab
    public GameObject acceleratorPrefab; // Reference to the accelerator prefab
    public GameObject refillStationPrefab; // Reference to the refill station prefab

    private GameObject currentObject; // Reference to the currently spawned object

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && currentObject != null && !buildingWindow.activeSelf)
        {
            DetachObject();
        }

        // Check for the "B" key press to toggle the building window
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBuildingWindow();
        }

        // Check for left mouse button press
        if (Input.GetMouseButtonDown(0) && currentObject != null)
        {
            FixateObject();
        }

        // Check for right mouse button press
        if (Input.GetMouseButtonDown(1) && currentObject != null && !buildingWindow.activeSelf)
        {
            DetachObject();
            ToggleBuildingWindow();
        }
    }

    void ToggleBuildingWindow()
    {
        // Toggle the visibility of the building window
        buildingWindow.SetActive(!buildingWindow.activeSelf);

        // If the window is closed, detach the current object
        if (!buildingWindow.activeSelf && currentObject != null)
        {
            DetachObject();
        }
    }

    // Method to handle the Capsule button click
    public void BuildCapsule()
    {
        // Ensure the teleporter prefab is not null
        if (teleporterPrefab != null)
        {
            buildingWindow.SetActive(!buildingWindow.activeSelf);

            // Build the teleporter using the teleporter prefab
            currentObject = BuildObject(teleporterPrefab);
        }
        else
        {
            Debug.LogError("Teleporter prefab is not assigned.");
        }
    }

    // Method to handle the Accelerator button click
    public void BuildAccelerator()
    {
        // Ensure the accelerator prefab is not null
        if (acceleratorPrefab != null)
        {
            buildingWindow.SetActive(!buildingWindow.activeSelf);

            // Build the accelerator using the accelerator prefab
            currentObject = BuildObject(acceleratorPrefab);
        }
        else
        {
            Debug.LogError("Accelerator prefab is not assigned.");
        }
    }

    // Method to handle the Refill Station button click
    public void BuildRefillStation()
    {
        // Ensure the refill station prefab is not null
        if (refillStationPrefab != null)
        {
            buildingWindow.SetActive(!buildingWindow.activeSelf);

            // Build the refill station using the refill station prefab
            currentObject = BuildObject(refillStationPrefab);
        }
        else
        {
            Debug.LogError("Refill station prefab is not assigned.");
        }
    }

    // Method to fixate the object position and rotation and detach it from the ship
    void FixateObject()
    {
        // Detach the object from the ship
        currentObject.transform.parent = null;

        // Disable any object-specific scripts or components here if needed

        // Set the object reference to null
        currentObject = null;
    }

    // Method to detach the current object from the ship
    void DetachObject()
    {
        if (currentObject != null)
        {
            // Detach the object from the ship
            currentObject.transform.parent = null;

            // Disable any object-specific scripts or components here if needed

            // Destroy the object
            Destroy(currentObject);

            // Set the object reference to null
            currentObject = null;
        }
    }

    // Method to instantiate a prefab and customize it
    private GameObject BuildObject(GameObject prefab)
    {
        // Calculate spawn position relative to the spawn transform (e.g., player's ship)
        Vector3 spawnPosition = transform.position + transform.forward * 5f;


        // Instantiate the prefab at the calculated spawn position
        GameObject newObj = Instantiate(prefab, spawnPosition, transform.rotation);

        newObj.transform.parent = transform;

        // Additional customization (e.g., setting opacity)
        SetObjectOpacity(newObj, 0.3f);

        return newObj;
    }

    // Method to set the opacity of an object
    private void SetObjectOpacity(GameObject obj, float opacity)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            Material objMaterial = objRenderer.material;
            Color materialColor = objMaterial.color;
            materialColor.a = opacity;
            objMaterial.color = materialColor;

            // Configure material for transparency
            objMaterial.SetFloat("_Mode", 3);
            objMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            objMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            objMaterial.SetInt("_ZWrite", 0);
            objMaterial.DisableKeyword("_ALPHATEST_ON");
            objMaterial.EnableKeyword("_ALPHABLEND_ON");
            objMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            objMaterial.renderQueue = 3000;
        }
    }
}
