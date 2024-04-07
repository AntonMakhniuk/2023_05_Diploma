using UnityEngine;
using System.Collections.Generic;

public class Teleporter : BuildingObject
{
    // Static list to store all teleporters
    public static List<Teleporter> allTeleporters = new List<Teleporter>();
    public GameObject teleporterPrefab; 

    private void Awake()
    {
        base.Awake();
        allTeleporters.Add(this);
    }

    private void OnDestroy()
    {
        allTeleporters.Remove(this);
    }



    private void OnTriggerEnter(Collider other)
    {
        TeleporterUI teleporterUI = TeleporterUI.GetInstance();
        if (other.CompareTag("Player") && teleporterUI != null)
        {
            // Create a list excluding the player's ship teleporter
            List<Teleporter> filteredTeleporters = new List<Teleporter>(allTeleporters);
            filteredTeleporters.Remove(this); // Remove the current teleporter (player's ship)

            // Prompt the player to choose a destination teleporter
            teleporterUI.ShowTeleporterUI(filteredTeleporters, this);
        }
    }
}