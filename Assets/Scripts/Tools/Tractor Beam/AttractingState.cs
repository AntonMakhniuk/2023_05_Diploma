using UnityEngine;

namespace Tools.Tractor_Beam
{
    public class AttractingState : BaseTractorBeamState
    {
        private const float MaxAttractionSpeed = 10f;
        private const float AttractionAcceleration = 3f;
        private const float HoldDistance = 2f;

        private float _currentSpeed;
        
        public AttractingState(TractorBeam context) : base(context)
        {
        }

        public override void Enter()
        {
            _currentSpeed = 0f;
        }

        public override void Update()
        {
            _currentSpeed = Mathf.Min(_currentSpeed + AttractionAcceleration * Time.deltaTime, MaxAttractionSpeed);

            var holdPosition = Context.holdPoint.position;
            var objectPosition = Context.AttractedObject.position;
            var distance = Vector3.Distance(holdPosition, objectPosition);
            
            //TODO: not robust against low fps due to using deltaTime for physics
            Context.AttractedObject.position = 
                Vector3.MoveTowards(Context.AttractedObject.position, 
                    holdPosition, _currentSpeed * Time.deltaTime);

            if (distance <= HoldDistance)
            {
                Context.SetState(TractorBeamState.Holding);
            }
        }

        public override void Exit()
        {
            // No special behavior
        }
    }
}