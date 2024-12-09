using System.Collections;
using Systems.Mining.Resource_Nodes.Base;
using Systems.Mining.Tools.Base_Tools;
using Tools.Base_Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Mining.Tools.Laser
{
    public class Laser : BaseTurret
    {
        [SerializeField] private float laserDamagePerSecond = 100f;
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
            Collider lastHitCollider = null;
            ResourceNodeWithHealth lastNodeWithHealth = null;
            ResourceNode lastNode = null;
            
            while (true)
            {
                var beamEndPosition = 
                    LookAtHitData?.point ?? muzzlePoint.position + muzzlePoint.transform.forward * maxRange;

                // Set the beam's positions
                beam.SetPosition(0, muzzlePoint.position);
                beam.SetPosition(1, beamEndPosition);
                
                if (LookAtHitData.HasValue && LookAtHitData.Value.collider != null)
                {
                    var currCollider = LookAtHitData.Value.collider;
                    
                    if (currCollider != lastHitCollider)
                    {
                        lastNodeWithHealth = null;
                        lastNode = null;
                        
                        if (currCollider.TryGetComponent<ResourceNodeWithHealth>(out var nodeWithHealth))
                        {
                            lastNodeWithHealth = nodeWithHealth;
                        }
                        else if (currCollider.TryGetComponent<ResourceNode>(out var node))
                        {
                            lastNode = node;
                        }

                        lastHitCollider = currCollider;
                    }
                        
                    if (lastNodeWithHealth != null)
                    {
                        lastNodeWithHealth.Interact(ToolType.Laser, laserDamagePerSecond * Time.deltaTime);
                    }
                    else if (lastNode != null)
                    {
                        lastNode.Interact(ToolType.Laser);
                    }
                }
                else
                {
                    lastHitCollider = null;
                    lastNodeWithHealth = null;
                    lastNode = null;
                }
                
                yield return null;
            }
        }
        
        protected override void OnActivate()
        {
            
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

        protected override void ScrollStarted(InputAction.CallbackContext ctx)
        {
            // No scroll action
        }

        protected override void ScrollPerformed(InputAction.CallbackContext ctx)
        {
            // No scroll action
        }

        protected override void ScrollCanceled(InputAction.CallbackContext ctx)
        {
            // No scroll action
        }

        protected override void OnDeactivate()
        {
            
        }
    }
}
