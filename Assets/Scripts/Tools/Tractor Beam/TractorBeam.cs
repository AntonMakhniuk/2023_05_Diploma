using System.Collections;
using System.Collections.Generic;
using Tools.Base_Tools;
using UnityEngine;

namespace Tools.Tractor_Beam
{
    public class TractorBeam : BaseTurret
    {
        public Rigidbody AttractedObject { get; set; }
        public Transform holdPoint;
        
        [SerializeField] private float maxAttractableMass = 100f;
        [SerializeField] private LayerMask attractableLayer;

        private Dictionary<TractorBeamState, BaseTractorBeamState> _stateDictionary = new();
        private BaseTractorBeamState _currentState;
        
        private IEnumerator _shootCoroutine;
        private bool _isShooting;
        
        private void Awake()
        {
            _stateDictionary = new Dictionary<TractorBeamState, BaseTractorBeamState>
            {
                { TractorBeamState.Idle, new IdleState(this) },
                { TractorBeamState.Attracting, new AttractingState(this) },
                { TractorBeamState.Holding, new HoldingState(this) },
                { TractorBeamState.Pushing, new PushingState(this) }
            };

            _shootCoroutine = ShootCoroutine();
            SetState(TractorBeamState.Idle);
        }

        private IEnumerator ShootCoroutine()
        {
            while (true)
            {
                if (_currentState is not IdleState)
                {
                    yield return null;
                    continue;
                }

                var beamRay = new Ray(muzzlePoint.position, muzzlePoint.forward);
                var castIntersect = Physics.Raycast(beamRay, out var hit, maxRange, attractableLayer);
            
                if (!castIntersect)
                {
                    yield return null;
                    continue;
                }

                if (!hit.collider.TryGetComponent<Rigidbody>(out var rb))
                {
                    yield return null;
                    continue;
                }

                if (rb.mass > maxAttractableMass)
                {
                    yield return null;
                    continue;
                }

                AttractedObject = rb;
                SetState(TractorBeamState.Attracting);

                yield return null;
            }
        }
        
        protected override void FixedWorkCycle()
        {
            _currentState.Update();
            
            base.FixedWorkCycle();
        }

        public void SetState(TractorBeamState newState)
        {
            _currentState?.Exit();
            _currentState = _stateDictionary[newState];
            _currentState.Enter();
        }

        protected override void PrimaryActionStarted()
        {
            if (_isShooting)
            {
                return;
            }

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
            
            StopCoroutine(_shootCoroutine);
            _isShooting = false;
            
            if (_currentState is not IdleState)
            {
                SetState(TractorBeamState.Idle);
            }
        }

        protected override void SecondaryActionStarted()
        {
            // No action on start
        }

        protected override void SecondaryActionPerformed()
        {
            if (_currentState is HoldingState)
            {
                SetState(TractorBeamState.Pushing);
            }
        }

        protected override void SecondaryActionCanceled()
        {
            // No action on cancel
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
