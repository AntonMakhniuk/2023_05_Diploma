using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractingState : ITractorBeamState
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

        Vector3 direction = (context.holdPoint.position - attractedObject.position).normalized;
        float distance = Vector3.Distance(context.holdPoint.position, attractedObject.position);
        float forceMagnitude = Mathf.Clamp(context.attractSpeed * distance, 0, context.attractSpeed);
        Vector3 force = direction * forceMagnitude;

        attractedObject.AddForce(force);

        if (distance <= context.holdDistance)
        {
            context.SetState(new HoldingState());
        }
    }

    public void ExitState(TractorBeamController context)
    {
    }
}