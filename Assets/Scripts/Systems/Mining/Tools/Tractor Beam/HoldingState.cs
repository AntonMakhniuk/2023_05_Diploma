using Systems.Mining.Tools.Tractor_Beam;
using UnityEngine;

namespace Tools.Tractor_Beam
{
    public class HoldingState : BaseTractorBeamState
    {
        private const float SmoothTime = 0.3f;
        private const float MaxSpeed = 10f;
        private const float RotationSpeed = 1.5f;

        private Vector3 _velocity = Vector3.zero;
        private Collider _attractedCollider;
        private Rigidbody _attractedRb;
        
        public HoldingState(TractorBeam context) : base(context)
        {
        }

        public override void Enter()
        {
            _attractedCollider = Context.AttractedObject.GetComponent<Collider>();
            _attractedRb = Context.AttractedObject.GetComponent<Rigidbody>();
            Context.AttractedObject.interpolation = RigidbodyInterpolation.Interpolate;
        }

        public override void Update()
        {
            var holdPosition = Context.holdPoint.position;
            var objectPosition = Context.AttractedObject.position;
            var targetPosition = objectPosition + (holdPosition - _attractedCollider.bounds.center);
            
            _attractedRb.MovePosition(Vector3.SmoothDamp(objectPosition, 
                targetPosition, ref _velocity, SmoothTime , MaxSpeed, Time.fixedDeltaTime));
            _attractedRb.MoveRotation(Quaternion.Slerp(Context.AttractedObject.rotation,
                Context.holdPoint.rotation, RotationSpeed * Time.fixedDeltaTime));
        }

        public override void Exit()
        {
            _attractedRb = null;
            _attractedCollider = null;
            _velocity = Vector3.zero;
            Context.AttractedObject.interpolation = RigidbodyInterpolation.None;
        }
    }
}