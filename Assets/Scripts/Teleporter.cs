using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private TeleporterUI teleporterUI; // Reference to the TeleporterUI script

    void Awake()
    {
        teleporterUI = FindObjectOfType<TeleporterUI>(); // Find the TeleporterUI script in the scene
        if (teleporterUI == null)
        {
            Debug.LogError("TeleporterUI script not found in the scene!");
        }
        else
        {
            DontDestroyOnLoad(gameObject); // Ensure the Teleporter object persists between scenes
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && teleporterUI != null)
        {
            // Prompt the player to choose a destination teleporter
            teleporterUI.ShowTeleporterUI(this);
        }
    }
}
