using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeamController : MonoBehaviour
{
    public float attractSpeed = 10f;
    public float holdDistance = 2f;
    public float pushForce = 20f;
    public Transform holdPoint;
    public LayerMask attractableLayer;

    private Rigidbody attractedObject;

    private ITractorBeamState currentState;

    private void Start()
    {
        currentState = new IdleState();
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SetState(ITractorBeamState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);
    }

    public void SetAttractedObject(Rigidbody obj)
    {
        attractedObject = obj;
    }

    public Rigidbody GetAttractedObject()
    {
        return attractedObject;
    }
}
