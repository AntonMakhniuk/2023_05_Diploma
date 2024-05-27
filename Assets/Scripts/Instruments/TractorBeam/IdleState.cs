using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : ITractorBeamState
{
    public void EnterState(TractorBeamController context)
    {
    }

    public void UpdateState(TractorBeamController context)
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(context.transform.position, context.transform.forward, out hit, 100f, context.attractableLayer))
            {
                Rigidbody rb = hit.rigidbody;
                if (rb != null)
                {
                    context.SetAttractedObject(rb);
                    context.SetState(new AttractingState());
                }
            }
        }
    }

    public void ExitState(TractorBeamController context)
    {
    }
}
