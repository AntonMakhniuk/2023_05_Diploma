using Resource_Nodes;
using Tools.Base_Tools;
using UnityEngine;

namespace Tools.Laser
{
    public class Laser : BaseTurret
    {
        private const float LaserDamagePerSecond = 10f;
        private const float MaxBeamLength = 100f;
    
        [SerializeField] private LineRenderer beam;

        private void Awake()
        {
            beam.enabled = false;
            beam.startWidth = 0.1f;
            beam.endWidth = 0.1f;
        }

        protected override void PrimaryActionStarted()
        {
            beam.enabled = true;
        }

        protected override void PrimaryActionPerformed()
        {
            var beamRay = new Ray(muzzlePoint.position, muzzlePoint.forward);
            var castIntersect = Physics.Raycast(beamRay, out var hit, MaxBeamLength);
            var beamEndPosition = castIntersect ? hit.point : 
                muzzlePoint.position + muzzlePoint.forward * MaxBeamLength;
            
            beam.SetPosition(0, muzzlePoint.position);
            beam.SetPosition(1, beamEndPosition);

            if (!castIntersect)
            {
                return;
            }
            
            if (hit.collider.TryGetComponent<IDestructible>(out var destructible))
            {
                destructible.OnLaserInteraction(LaserDamagePerSecond * Time.deltaTime);
            }
        }

        protected override void PrimaryActionCanceled()
        {
            beam.enabled = false;
            
            var position = muzzlePoint.position;
            beam.SetPosition(0, position);
            beam.SetPosition(1, position);
        }

        protected override void SecondaryActionStarted()
        {
            // No secondary action
        }

        protected override void SecondaryActionPerformed()
        {
            // No secondary action
        }

        protected override void SecondaryActionCanceled()
        {
            // No secondary action
        }

        protected override void ThirdActionStarted()
        {
            // No third action
        }

        protected override void ThirdActionPerformed()
        {
            // No third action
        }

        protected override void ThirdActionCanceled()
        {
            // No third action
        }
    }
}
