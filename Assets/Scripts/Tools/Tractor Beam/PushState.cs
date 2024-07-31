using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushState : ITractorBeamState
{
    public void EnterState(TractorBeamController context)
    {
        Rigidbody attractedObject = context.GetAttractedObject();

        if (attractedObject != null)
        {
            Vector3 pushDirection = (attractedObject.position - context.transform.position).normalized;
            attractedObject.AddForce(pushDirection * context.pushForce, ForceMode.Impulse);
            context.SetAttractedObject(null);
        }
    }

    public void UpdateState(TractorBeamController context)
    {
        context.SetState(new IdleState());
    }

    public void ExitState(TractorBeamController context)
    {
    }
}