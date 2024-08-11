using System.Collections;
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

        private IEnumerator _shootCoroutine;
        private bool _isShooting;
        
        private void Awake()
        {
            beam.enabled = false;
            beam.startWidth = 0.1f;
            beam.endWidth = 0.1f;

            _shootCoroutine = ShootCoroutine();
        }

        private IEnumerator ShootCoroutine()
        {
            while (true)
            {
                var beamRay = new Ray(muzzlePoint.position, muzzlePoint.forward);
                var castIntersect = Physics.Raycast(beamRay, out var hit, MaxBeamLength);
                var beamEndPosition =
                    castIntersect ? hit.point : muzzlePoint.position + muzzlePoint.forward * MaxBeamLength;

                beam.SetPosition(0, muzzlePoint.position);
                beam.SetPosition(1, beamEndPosition);

                if (castIntersect)
                {
                    if (hit.collider.TryGetComponent<IDestructible>(out var destructible))
                    {
                        destructible.OnLaserInteraction(LaserDamagePerSecond * Time.deltaTime);
                    }
                }
                
                yield return null;
            }
        }
        
        protected override void PrimaryActionStarted()
        {
            if (_isShooting)
            {
                return;
            }
            
            beam.enabled = true;
            StartCoroutine(_shootCoroutine);
            _isShooting = true;
        }

        protected override void PrimaryActionPerformed()
        {
            // No action on performed
        }

        protected override void PrimaryActionCanceled()
        {
            if (!_isShooting)
            {
                return;
            }
            
            beam.enabled = false;
            StopCoroutine(_shootCoroutine);
            _isShooting = false;
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
