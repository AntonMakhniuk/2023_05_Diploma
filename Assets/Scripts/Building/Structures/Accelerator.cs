using System.Collections;
using Building.Systems;
using Player.Movement;
using UnityEngine;

namespace Building.Structures
{
    //TODO: Make work for non-player ships if we do add them
    public class Accelerator : BuildingObject
    {
        //Set to <=0 to unconstrain
        private const int MaxAccelerationCount = 0;
        private const float AccelerationPercent = 1.2f;
        private const float AccelerationLength = 3f;
        
        private static int _accelerationCount;
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SpeedBoost();
            }
        }

        private void SpeedBoost()
        {
            if (MaxAccelerationCount > 0 && _accelerationCount >= MaxAccelerationCount)
            {
                return;
            }

            StartCoroutine(SpeedBoostCoroutine());
        }

        private static IEnumerator SpeedBoostCoroutine()
        {
            PlayerMovement.Instance.SpeedModifier *= AccelerationPercent;
                
            _accelerationCount++;

            yield return new WaitForSeconds(AccelerationLength);

            PlayerMovement.Instance.SpeedModifier /= AccelerationPercent;

            _accelerationCount--;
        }
    }
}
