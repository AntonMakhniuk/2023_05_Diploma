using UnityEngine;

public class BuildingWindow : MonoBehaviour
{
    public GameObject buildingWindow; // Reference to the building window Panel
    public GameObject teleporterPrefab; // Reference to the capsule prefab
    public GameObject acceleratorPrefab; // Reference to the accelerator prefab

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
        // Toggle the building window
        buildingWindow.SetActive(!buildingWindow.activeSelf);

        // Instantiate the capsule or accelerator prefab in front of the ship
        Vector3 spawnPosition = transform.position + transform.forward * 5f;
        currentObject = Instantiate(teleporterPrefab, spawnPosition, transform.rotation);

        // Set the ship as the parent of the object
        currentObject.transform.parent = transform;

        // Set opacity if needed
        SetObjectOpacity(currentObject, 0.3f);
    }

    // Method to handle the Accelerator button click
    public void BuildAccelerator()
    {
        // Toggle the building window
        buildingWindow.SetActive(!buildingWindow.activeSelf);

        // Instantiate the accelerator prefab in front of the ship
        Vector3 spawnPosition = transform.position + transform.forward * 5f;
        currentObject = Instantiate(acceleratorPrefab, spawnPosition, transform.rotation);

        // Set the ship as the parent of the object
        currentObject.transform.parent = transform;

        // Set opacity if needed
        SetObjectOpacity(currentObject, 0.3f);
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
        // Detach the object from the ship
        currentObject.transform.parent = null;

        // Disable any object-specific scripts or components here if needed

        // Destroy the object
        Destroy(currentObject);

        // Set the object reference to null
        currentObject = null;
    }

    void SetObjectOpacity(GameObject obj, float opacity)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            // Get the current material of the object
            Material objMaterial = objRenderer.material;

            // Set the alpha (transparency) of the material
            Color materialColor = objMaterial.color;
            materialColor.a = opacity;
            objMaterial.color = materialColor;

            // Enable alpha blending for transparency
            objMaterial.SetFloat("_Mode", 3);
            objMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            objMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            objMaterial.SetInt("_ZWrite", 0);
            objMaterial.DisableKeyword("_ALPHATEST_ON");
            objMaterial.EnableKeyword("_ALPHABLEND_ON");
            objMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            objMaterial.renderQueue = 3000; // or another value depending on your needs
        }
    }
}
