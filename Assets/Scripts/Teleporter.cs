using UnityEngine;
using System.Collections.Generic;

public class Teleporter : MonoBehaviour
{
    // Static list to store all teleporters
    public static List<Teleporter> allTeleporters = new List<Teleporter>();

    private void Awake()
    {
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
            Debug.Log("ZALUPKO");

            // Prompt the player to choose a destination teleporter
            teleporterUI.ShowTeleporterUI(allTeleporters, this);
        }
    }
}
