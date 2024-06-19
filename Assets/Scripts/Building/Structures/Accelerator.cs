using Building.Systems;
using Player;
using Scriptable_Object_Templates.Building;
using UnityEngine;

namespace Building.Structures
{
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
                //inputSystem.SpeedBoost();
            }
            else
            {
                Debug.LogWarning("MovementInputSystem component not found on player.");
            }
        }

    }
}
