using UnityEngine;
using System.Collections;

public class Accelerator : BuildingObject
{

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered refill station trigger area.");
            SpeedBoost(other.gameObject);
        }
    }

    private void SpeedBoost(GameObject player)
    {
        MovementInputSystem inputSystem = player.GetComponent<MovementInputSystem>();

        if (inputSystem != null)
        {
            inputSystem.SpeedBoost();
        }
        else
        {
            Debug.LogWarning("MovementInputSystem component not found on player.");
        }
    }

}