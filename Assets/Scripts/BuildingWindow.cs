using UnityEngine;

public class BuildingWindow : MonoBehaviour
{
    public GameObject buildingWindow; // Reference to the building window Panel
    public GameObject capsulePrefab; // Reference to the capsule prefab

    private GameObject currentCapsule; // Reference to the currently spawned capsule

    void Update()
    {
        // Check for the "B" key press to toggle the building window
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBuildingWindow();
        }

        // Check for left mouse button press
        if (Input.GetMouseButtonDown(0) && currentCapsule != null)
        {
            FixateCapsule();
        }

        if (Input.GetMouseButtonDown(1) && currentCapsule != null && !buildingWindow.activeSelf)
        {
            DetachCapsule();
            ToggleBuildingWindow();
        }
    }

    void ToggleBuildingWindow()
    {
        // Toggle the visibility of the building window
        buildingWindow.SetActive(!buildingWindow.activeSelf);

        // If the window is closed, detach the current capsule
        if (!buildingWindow.activeSelf && currentCapsule != null)
        {
            DetachCapsule();
        }
    }

    // Method to handle the Capsule button click
    public void BuildCapsule()
    {

        buildingWindow.SetActive(!buildingWindow.activeSelf);
        // Instantiate the capsule prefab in front of the ship
        Vector3 spawnPosition = transform.position + transform.forward * 5f;
        currentCapsule = Instantiate(capsulePrefab, spawnPosition, Quaternion.identity);

        // Set the ship as the parent of the capsule
        currentCapsule.transform.parent = transform;
        SetCapsuleOpacity(currentCapsule, 0.3f);
    }

    // Method to fixate the capsule position and rotation and detach it from the ship
    void FixateCapsule()
    {
        // Detach the capsule from the ship
        currentCapsule.transform.parent = null;

        // Disable any capsule-specific scripts or components here if needed

        // Set the capsule as the currentCapsule to null
        currentCapsule = null;
    }

    // Method to detach the current capsule from the ship
    void DetachCapsule()
    {
        // Detach the capsule from the ship
        currentCapsule.transform.parent = null;

        // Disable any capsule-specific scripts or components here if needed

        // Destroy the capsule
        Destroy(currentCapsule);

        // Set the currentCapsule to null
        currentCapsule = null;
    }

    void SetCapsuleOpacity(GameObject capsule, float opacity)
    {
        Renderer capsuleRenderer = capsule.GetComponent<Renderer>();
        if (capsuleRenderer != null)
        {
            // Get the current material of the capsule
            Material capsuleMaterial = capsuleRenderer.material;

            // Set the alpha (transparency) of the material
            Color materialColor = capsuleMaterial.color;
            materialColor.a = opacity;
            capsuleMaterial.color = materialColor;

            // Enable alpha blending for transparency
            capsuleMaterial.SetFloat("_Mode", 3);
            capsuleMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            capsuleMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            capsuleMaterial.SetInt("_ZWrite", 0);
            capsuleMaterial.DisableKeyword("_ALPHATEST_ON");
            capsuleMaterial.EnableKeyword("_ALPHABLEND_ON");
            capsuleMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            capsuleMaterial.renderQueue = 3000; // or another value depending on your needs
        }
    }
}
