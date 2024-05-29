using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingState : ITractorBeamState
{
    public void EnterState(TractorBeamController context)
    {
    }

    public void UpdateState(TractorBeamController context)
    {
        Rigidbody attractedObject = context.GetAttractedObject();

        if (attractedObject == null)
        {
            context.SetState(new IdleState());
            return;
        }

        attractedObject.velocity = Vector3.zero;
        attractedObject.position = context.holdPoint.position;
    }

    public void ExitState(TractorBeamController context)
    {
    }
}