using UnityEngine;
using UnityEngine.UI;

public class TeleporterUI : MonoBehaviour
{
    public GameObject teleporterButtonPrefab;
    private GameObject currentTeleporterUI;

    public void ShowTeleporterUI(Teleporter teleporter)
    {
        // Destroy any existing teleporter UI
        Destroy(currentTeleporterUI);

        // Instantiate the teleporter UI prefab
        currentTeleporterUI = InstantiateTeleporterUI();

        // Get all teleporters
        Teleporter[] teleporters = FindObjectsOfType<Teleporter>();

        foreach (Teleporter destTeleporter in teleporters)
        {
            if (destTeleporter != teleporter)
            {
                // Create a button for each destination teleporter
                GameObject buttonGO = Instantiate(teleporterButtonPrefab, currentTeleporterUI.transform);
                Button button = buttonGO.GetComponent<Button>();
                Text buttonText = buttonGO.GetComponentInChildren<Text>();
                buttonText.text = "Teleport to: " + destTeleporter.transform.position.ToString();

                // Add an onclick event to the button
                button.onClick.AddListener(() => TeleportToDestination(destTeleporter));
            }
        }
    }

    private GameObject InstantiateTeleporterUI()
    {
        GameObject teleporterUI = new GameObject("TeleporterUI");
        RectTransform rectTransform = teleporterUI.AddComponent<RectTransform>();
        Canvas canvas = teleporterUI.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        GridLayoutGroup gridLayout = teleporterUI.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(200, 50); // Adjust button size as needed
        teleporterUI.AddComponent<GraphicRaycaster>();

        return teleporterUI;
    }

    private void TeleportToDestination(Teleporter destination)
    {
        // Teleport the player to the destination teleporter
        GameObject playerShip = GameObject.FindGameObjectWithTag("Player");
        Vector3 offset = destination.transform.forward * 3f; // Adjust the distance as needed
        Vector3 teleportPosition = destination.transform.position + offset;
        playerShip.transform.position = teleportPosition;

        // Destroy the teleporter UI
        Destroy(currentTeleporterUI);
    }
}
