using System;
using UI.Systems.Interactables;
using UnityEngine;

namespace Player
{
    public class CameraInteractable : MonoBehaviour
    {
        public static float InteractionRange;
        
        private void Update()
        {
            Physics.Raycast(transform.position, transform.forward, out var hit, 
                InteractionRange, LayerMask.GetMask("Interactable"), QueryTriggerInteraction.Ignore);
            
            hit.transform.GetComponent<Interactable>().HandleCameraLooking();
        }
    }
}