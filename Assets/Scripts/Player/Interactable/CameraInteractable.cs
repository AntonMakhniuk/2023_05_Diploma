using UnityEngine;

namespace Player.Interactable
{
    public class CameraInteractable : MonoBehaviour
    {
        [SerializeField] private float interactionRange;

        private Transform _previousHit;
        
        private void Update()
        {
            if (!Physics.Raycast(transform.position, transform.forward, out var hit,
                    interactionRange, LayerMask.GetMask("Interactable"), QueryTriggerInteraction.Ignore))
            {
                return;
            }

            // Avoiding doing unnecessary GetComponent calls
            if (_previousHit == hit.transform)
            {
                return;
            }
            
            hit.transform.GetComponent<UI.Systems.Interactables.Interactable>().HandleCameraLooking();

            _previousHit = hit.transform;
        }
    }
}