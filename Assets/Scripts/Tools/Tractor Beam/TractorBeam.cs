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
        
        private const float MaxBeamRange = 100f;
        private const float MaxAttractableMass = 100f;
        
        [SerializeField] private LayerMask attractableLayer;

        private Dictionary<TractorBeamState, BaseTractorBeamState> _stateDictionary = new();
        private BaseTractorBeamState _currentState;
        private IdleState _idleState;
        private AttractingState _attractingState;
        private HoldingState _holdingState;
        private PushState _pushState;
        
        private void Awake()
        {
            _idleState = new IdleState(this);
            _attractingState = new AttractingState(this);
            _holdingState = new HoldingState(this);
            _pushState = new PushState(this);
            
            _stateDictionary = new Dictionary<TractorBeamState, BaseTractorBeamState>
            {
                { TractorBeamState.Idle, _idleState },
                { TractorBeamState.Attracting, _attractingState },
                { TractorBeamState.Holding, _holdingState },
                { TractorBeamState.Pushing, _pushState }
            };

            SetState(TractorBeamState.Idle);
        }

        protected override IEnumerator WorkCoroutine()
        {
            _currentState.Update();
            
            return base.WorkCoroutine();
        }

        public void SetState(TractorBeamState newState)
        {
            _currentState?.Exit();
            _currentState = _stateDictionary[newState];
            _currentState.Enter();
        }

        protected override void PrimaryActionStarted()
        {
            if (_currentState is not IdleState)
            {
                return;
            }
            
            var beamRay = new Ray(muzzlePoint.position, muzzlePoint.forward);
            var castIntersect = Physics.Raycast(beamRay, out var hit, MaxBeamRange, attractableLayer);
            
            if (!castIntersect)
            {
                return;
            }

            if (!hit.collider.TryGetComponent<Rigidbody>(out var rb))
            {
                return;
            }
            
            if (rb.mass > MaxAttractableMass)
            {
                return;
            }

            AttractedObject = rb;
            SetState(TractorBeamState.Attracting);
        }

        protected override void PrimaryActionPerformed()
        {
            // No action on performed
        }

        protected override void PrimaryActionCanceled()
        {
            SetState(TractorBeamState.Idle);
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
