using UnityEngine;

namespace Tools.Tractor_Beam
{
    public class AttractingState : BaseTractorBeamState
    {
        // Dictates how fast an object will be attracted to the tractor beam
        private const float MaxAttractionSpeed = 15f;
        private const float AttractionAcceleration = 3f;
        
        // A small "forgiveness" value so that the attracted object position allows
        // for a small amount of wiggle room when transitioning to a hold state
        private const float HoldDistanceForgiveness = 0.1f;
        
        // How fast the object is rotated while being attracted
        private const float RotationSpeed = 0.5f;
        
        // The minimum time it should take for an object to be attracted
        private const float MinSmoothTime = 0.1f;

        private float _currentSpeed;
        private Collider _attractedCollider;
        private Rigidbody _attractedRb;
        private Vector3 _velocity = Vector3.zero;

        
        public AttractingState(TractorBeam context) : base(context)
        {
        }

        public override void Enter()
        {
            _currentSpeed = 0f;
            _attractedCollider = Context.AttractedObject.GetComponent<Collider>();
            _attractedRb = Context.AttractedObject.GetComponent<Rigidbody>();
        }

        public override void Update()
        {
            _currentSpeed = Mathf.Min(_currentSpeed + AttractionAcceleration * Time.deltaTime, MaxAttractionSpeed);
            
            var holdPosition = Context.holdPoint.position;
            var objectPosition = Context.AttractedObject.position;
            var targetPosition = objectPosition + (holdPosition - _attractedCollider.bounds.center);
            
            var distanceToTarget = Vector3.Distance(objectPosition, targetPosition);
            var smoothTime = 
                Mathf.Max(Mathf.Sqrt(2 * distanceToTarget / AttractionAcceleration), MinSmoothTime);
            
            _attractedRb.MovePosition(Vector3.SmoothDamp(Context.AttractedObject.position, 
                targetPosition, ref _velocity, smoothTime , _currentSpeed, Time.fixedDeltaTime));
            _attractedRb.MoveRotation(Quaternion.Slerp(Context.AttractedObject.rotation,
                Context.holdPoint.rotation, RotationSpeed * Time.fixedDeltaTime));
            
            if (distanceToTarget <= HoldDistanceForgiveness)
            {
                Context.SetState(TractorBeamState.Holding);
            }
        }

        public override void Exit()
        {
            _currentSpeed = 0f;
            _attractedCollider = null;
            _attractedRb = null;
            _velocity = Vector3.zero;
        }
    }
}