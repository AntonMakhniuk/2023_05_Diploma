using UnityEngine;

namespace Tools.Tractor_Beam
{
    public class HoldingState : BaseTractorBeamState
    {
        public HoldingState(TractorBeam context) : base(context)
        {
        }

        public override void Enter()
        {
            // No special behavior
        }

        public override void Update()
        {
            Context.AttractedObject.position = Context.holdPoint.position;
            Context.AttractedObject.velocity = Vector3.zero;
        }

        public override void Exit()
        {
            // No special behavior
        }
    }
}