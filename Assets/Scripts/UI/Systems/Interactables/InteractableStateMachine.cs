﻿using UI.Systems.Interactables.States;
using UnityEngine;

namespace UI.Systems.Interactables
{
    public abstract class InteractableStateMachine : MonoBehaviour
    {
        protected InteractableState State;

        protected void SetState(InteractableState newState)
        {
            Debug.Log("State changed: " + newState);
            
            State = newState;
            StartCoroutine(State.UpdateDisplay());
        }
    }
}