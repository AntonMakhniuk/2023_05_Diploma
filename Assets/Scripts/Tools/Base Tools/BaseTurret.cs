using UnityEngine;

namespace Tools.Base_Tools
{
    public abstract class BaseTurret : BaseTool
    {
        [Header("Turret sections; leg can be left null for \"grounded\" turrets.")]
        [SerializeField] private Transform turretBase;
        [SerializeField] private Transform turretLeg;
        [SerializeField] private Transform turretBarrel;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private RectTransform crosshairPos;
        
        [SerializeField] protected float maxRange;
        [SerializeField] protected Transform muzzlePoint;

        protected RaycastHit? LookAtHitData;

        private bool _hasSeparateLeg;

        protected override void Start()
        {
            _hasSeparateLeg = turretLeg != null;
            
            base.Start();
        }
        
        protected override void WorkCycle()
        {
            RotateWithCamera();
            
            base.WorkCycle();
        }

        // Rotate the turret to match the direction of the camera
        private void RotateWithCamera() 
        {
            var screenRay = Camera.main.ScreenPointToRay(crosshairPos.position);
    
            var hasHit = Physics.Raycast(screenRay, out var hit, maxRange);
            LookAtHitData = hasHit ? hit : null;
    
            var targetPoint = hasHit ? hit.point : screenRay.GetPoint(maxRange);
            var relativeTargetPoint = turretBase.transform.parent.InverseTransformPoint(targetPoint);

            // Calculate rotation around the Z-axis for leg rotation
            var zRotationAngle = Mathf.Atan2(relativeTargetPoint.y, relativeTargetPoint.x) * Mathf.Rad2Deg;
            var targetZRotation = Quaternion.Euler(0f, 0f, zRotationAngle);

            if (_hasSeparateLeg)
            {
                // Rotate the leg on its Z-axis
                turretLeg.transform.localRotation = Quaternion.Lerp(turretLeg.transform.localRotation, 
                    targetZRotation, Time.deltaTime * rotationSpeed);
            }
            else
            {
                turretBase.transform.localRotation = Quaternion.Lerp(turretBase.transform.localRotation, 
                    targetZRotation, Time.deltaTime * rotationSpeed);
            }
    
            // Calculate pitch for the barrel (up and down rotation)
            var pitchAngle = -Mathf.Atan2(relativeTargetPoint.y, 
                Mathf.Sqrt(relativeTargetPoint.x * relativeTargetPoint.x + relativeTargetPoint.z * relativeTargetPoint.z)) * Mathf.Rad2Deg;
            var targetPitchRotation = Quaternion.Euler(pitchAngle, 0f, 0f);
    
            turretBarrel.transform.localRotation = Quaternion.Lerp(turretBarrel.transform.localRotation, 
                targetPitchRotation, Time.deltaTime * rotationSpeed);
        }
    }
}