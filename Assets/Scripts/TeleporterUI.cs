using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TeleporterUI : MonoBehaviour
{
    public GameObject teleporterButtonPrefab;
    private GameObject currentTeleporterUI;

    private static TeleporterUI instance;

    private void Awake()
    {
        instance = this;
    }

    public static TeleporterUI GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        HideTeleporterUI();
    }

    public void ShowTeleporterUI(List<Teleporter> allTeleporters, Teleporter selectedTeleporter)
    {
        Destroy(currentTeleporterUI);
       

        currentTeleporterUI = InstantiateTeleporterUI();

        foreach (Teleporter destTeleporter in allTeleporters)
        {
            if (destTeleporter != selectedTeleporter)
            {

                GameObject buttonGO = Instantiate(teleporterButtonPrefab, currentTeleporterUI.transform);
                Button button = buttonGO.GetComponent<Button>();
                Text buttonText = buttonGO.GetComponentInChildren<Text>();
                buttonText.text = "Teleport to: " + destTeleporter.transform.position.ToString();

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
        gridLayout.cellSize = new Vector2(200, 50); 
        teleporterUI.AddComponent<GraphicRaycaster>();

        return teleporterUI;
    }

    private void TeleportToDestination(Teleporter destination)
    {
       
        GameObject playerShip = GameObject.FindGameObjectWithTag("Player");
        Vector3 offset = destination.transform.forward * 3f; 
        Vector3 teleportPosition = destination.transform.position + offset;
        playerShip.transform.position = teleportPosition;

        HideTeleporterUI();
    }

    public void HideTeleporterUI()
    {
        if (currentTeleporterUI != null)
        {
            Destroy(currentTeleporterUI);
            currentTeleporterUI = null;
        }
    }
}