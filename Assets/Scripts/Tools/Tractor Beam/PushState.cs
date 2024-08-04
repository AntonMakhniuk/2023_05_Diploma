using UnityEngine;

namespace Tools.Tractor_Beam
{
    public class PushState : BaseTractorBeamState
    {
        private const float PushForce = 20f;
        
        public PushState(TractorBeam context) : base(context)
        {
        }
        
        public override void Enter()
        {
            var pushDirection = (Context.AttractedObject.position - Context.transform.position).normalized;
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