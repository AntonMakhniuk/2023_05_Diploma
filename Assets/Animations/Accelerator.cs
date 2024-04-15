using UnityEngine;
using System.Collections;

public class Accelerator : BuildingObject
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered refill station trigger area.");

            // Show UI prompt to refill fuel
            // if (refillPromptText != null)
            {
                //  refillPromptText.gameObject.SetActive(true);
            }

            SpeedBoost(other.gameObject);
        }
    }

    private void SpeedBoost(GameObject player)
    {
        MovementInputSystem inputSystem = player.GetComponent<MovementInputSystem>();

        if (inputSystem != null)
        {
            Debug.Log("MovementInputSystem component found on player.");
            inputSystem.SpeedBoost();
        }
        else
        {
            Debug.LogWarning("MovementInputSystem component not found on player.");
        }
    }

}
