
using UnityEngine;

namespace Testing
{
    public abstract class DroneBaseTurret : DronBaseTool
    {
        [Header("Turret sections; leg can be left null for \"grounded\" turrets.")] [SerializeField]
        private Transform turretBase;

        [SerializeField] private Transform turretLeg;
        [SerializeField] private Transform turretBarrel;
        [SerializeField] private float rotationSpeed;

        [SerializeField] protected float maxRange;
        [SerializeField] protected Transform muzzlePoint;
        [SerializeField] private RectTransform crosshairUI;
        
        protected RaycastHit? LookAtHitData;

        private bool _hasSeparateLeg;

        protected override void Start()
        {
            if (turretLeg == null)
            {
                _hasSeparateLeg = false;
            }

            base.Start();
        }

        protected override void WorkCycle()
        {
            //RotateWithCamera();

            base.WorkCycle();
        }

        // Rotate the turret to match the direction of the camera
        private void RotateWithCamera()
        {
            

            var screenRay = Camera.main.ScreenPointToRay(crosshairUI.position);

            var hasHit = Physics.Raycast(screenRay, out var hit, maxRange);
            LookAtHitData = hasHit ? hit : null;

            var targetPoint = hasHit ? hit.point : screenRay.GetPoint(maxRange);
            var relativeTargetPoint = turretBase.transform.parent.InverseTransformPoint(targetPoint);

            var yawAngle = Mathf.Atan2(relativeTargetPoint.x, relativeTargetPoint.z) * Mathf.Rad2Deg;
            var targetYawRotation = Quaternion.Euler(0f, yawAngle, 0f);

            if (_hasSeparateLeg)
            {
                turretLeg.transform.localRotation = Quaternion.Lerp(turretLeg.transform.localRotation,
                    targetYawRotation, Time.deltaTime * rotationSpeed);
            }
            else
            {
                turretBase.transform.localRotation = Quaternion.Lerp(turretBase.transform.localRotation,
                    targetYawRotation, Time.deltaTime * rotationSpeed);
            }

            var pitchAngle = -Mathf.Atan2(relativeTargetPoint.y,
                Mathf.Sqrt(relativeTargetPoint.x * relativeTargetPoint.x
                           + relativeTargetPoint.z * relativeTargetPoint.z)) * Mathf.Rad2Deg;
            var targetPitchRotation = Quaternion.Euler(pitchAngle, 0f, 0f);

            turretBarrel.transform.localRotation = Quaternion.Lerp(turretBarrel.transform.localRotation,
                targetPitchRotation, Time.deltaTime * rotationSpeed);
        }
    }
}