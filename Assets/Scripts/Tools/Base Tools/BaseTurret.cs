using System.Collections;
using Player.Ship;
using UnityEngine;

namespace Tools.Base_Tools
{
    public abstract class BaseTurret : BaseTool
    {
        [SerializeField] private Transform turretBase;
        [SerializeField] private Transform turretLeg;
        [SerializeField] private Transform turretBarrel;
        
        [SerializeField] protected Transform muzzlePoint;

        protected override IEnumerator WorkCoroutine()
        {
            while (true)
            {
                RotateWithCamera();

                yield return null;
            }
        }

        // Rotate the turret to match the direction of the camera
        private void RotateWithCamera() 
        {
            var basePosition = turretBase.transform.position;
            
            var cameraPosition = cinematicCamera.transform.position;
            var cameraForward = cinematicCamera.transform.forward;
            
            var direction = cameraPosition + cameraForward * 100f - basePosition;
            var relativeDirection = turretBase.transform.parent.InverseTransformDirection(direction);
            
            var legAngle = Mathf.Atan2(relativeDirection.x, relativeDirection.z) * Mathf.Rad2Deg;
            turretLeg.transform.localRotation = Quaternion.Euler(0f, legAngle, 0f);
            
            var barrelAngle = -Mathf.Atan2(relativeDirection.y, 
                Mathf.Sqrt(relativeDirection.x * relativeDirection.x + relativeDirection.z * relativeDirection.z))
                              * Mathf.Rad2Deg;
            turretBarrel.transform.localRotation = Quaternion.Euler(barrelAngle, 0f, 0f);
        }
    }
}