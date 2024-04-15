using UnityEngine;

public class BuildingWindow : MonoBehaviour
{
    public GameObject buildingWindow;
    public BuildingManager buildingManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && buildingManager.CurrentObject != null && !buildingWindow.activeSelf)
        {
            buildingManager.RemoveCurrentObject();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBuildingWindow();
        }

        if (Input.GetMouseButtonDown(0) && buildingManager.CurrentObject != null)
        {
            buildingManager.FixateObject();
        }

        if (Input.GetMouseButtonDown(1) && buildingManager.CurrentObject != null && !buildingWindow.activeSelf)
        {
            buildingManager.RemoveCurrentObject();
            ToggleBuildingWindow();
        }
    }

    private void ToggleBuildingWindow()
    {
        buildingWindow.SetActive(!buildingWindow.activeSelf);

        if (!buildingWindow.activeSelf && buildingManager.CurrentObject != null)
        {
            buildingManager.RemoveCurrentObject();
        }
    }

    public void BuildTeleporter()
    {
        if (buildingManager != null && buildingManager.teleporterPrefab != null)
        {
            buildingManager.BuildObject(buildingManager.teleporterPrefab);
            buildingWindow.SetActive(false);
        }
        else
        {
            Debug.LogError("Building manager or Teleporter prefab is not assigned.");
        }
    }

    public void BuildAccelerator()
    {
        if (buildingManager != null && buildingManager.acceleratorPrefab != null)
        {
            buildingManager.BuildObject(buildingManager.acceleratorPrefab);
            buildingWindow.SetActive(false);
        }
        else
        {
            Debug.LogError("Building manager or Accelerator prefab is not assigned.");
        }
    }

    public void BuildRefillStation()
    {
        if (buildingManager != null && buildingManager.refillStationPrefab != null)
        {
            buildingManager.BuildObject(buildingManager.refillStationPrefab);
            buildingWindow.SetActive(false);
        }
        else
        {
            Debug.LogError("Building manager or Refill station prefab is not assigned.");
        }
    }
}
