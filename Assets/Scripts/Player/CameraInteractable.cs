using UI.Systems.Interactables;
using UnityEngine;

namespace Player
{
    public class CameraInteractable : MonoBehaviour
    {
        [SerializeField] private float interactionRange;
        
        private void Update()
        {
            if (!Physics.Raycast(transform.position, transform.forward, out var hit,
                    interactionRange, LayerMask.GetMask("Interactable"), QueryTriggerInteraction.Ignore))
            {
                return;
            }
            
            hit.transform.GetComponent<Interactable>().HandleCameraLooking();
        }
    }
}