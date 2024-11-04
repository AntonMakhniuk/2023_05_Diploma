using UnityEngine;

namespace Tools.Tractor_Beam
{
    public class PushingState : BaseTractorBeamState
    {
        private const float PushForce = 20f;
        
        public PushingState(TractorBeam context) : base(context)
        {
        }
        
        public override void Enter()
        {
            var pushDirection = (Context.AttractedObject.position - Context.holdPoint.position).normalized;
            Context.AttractedObject.AddForce(pushDirection * PushForce, ForceMode.Impulse);
            Context.AttractedObject = null;
            Context.SetState(TractorBeamState.Idle);
        }

        public override void Update()
        {
            // No special behavior
        }

        public override void Exit()
        {
            // No special behavior
        }
    }
}