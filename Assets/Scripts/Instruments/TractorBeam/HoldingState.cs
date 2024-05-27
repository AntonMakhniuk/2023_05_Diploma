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

        if (Input.GetMouseButtonDown(1))
        {
            context.SetAttractedObject(null);
            context.SetState(new IdleState());
        }
        else if (Input.GetMouseButtonDown(2)) 
        {
            context.SetState(new PushState());
        }
    }

    public void ExitState(TractorBeamController context)
    {
    }
}
