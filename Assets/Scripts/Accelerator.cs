using UnityEngine;
using System.Collections;

public class Accelerator : BuildingObject
{

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpeedBoost(other.gameObject);
        }
    }

    private void SpeedBoost(GameObject player)
    {
        PlayerMovement inputSystem = player.GetComponent<PlayerMovement>();

        if (inputSystem != null)
        {
            // TODO: uncomment this
            //inputSystem.SpeedBoost();
        }
        else
        {
            Debug.LogWarning("MovementInputSystem component not found on player.");
        }
    }

}
