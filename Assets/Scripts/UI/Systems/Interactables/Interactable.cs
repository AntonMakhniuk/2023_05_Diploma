using Player;
using UI.Systems.Interactables.States;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Systems.Interactables
{
    public class Interactable : InteractableStateMachine
    {
        public Image objectIconImage, bindIconImage;
        private Sprite _objectIcon, _bindIcon;
        public Sprite ObjectIcon
        {
            get => _objectIcon;
            set
            {
                _objectIcon = value;
                objectIconImage.sprite = _objectIcon;
            }
        }
        public Sprite BindIcon
        {
            get => _bindIcon;
            set
            {
                _bindIcon = value;
                bindIconImage.sprite = _bindIcon;
            }
        }
        
        public UnityEvent onInteractionStarted, onInteractionEnded;

        private void Start()
        {
            State = new Hidden(this);
            PlayerActions.InputActions.UI.InteractWithObject.performed += ToggleActivated;
        }

        public void HandleCameraLooking()
        {
            if (State is Shown)
            {
                SetState(new Highlit(this));
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            SetState(new Shown(this));
        }

        private void OnTriggerExit(Collider other)
        {
            SetState(new Hidden(this));
        }

        private void ToggleActivated(InputAction.CallbackContext callbackContext)
        {
            if (State is not Activated)
            {
                SetState(new Activated(this));
                onInteractionStarted.Invoke();
            }
            else
            {
                SetState(new Highlit(this));
                onInteractionEnded.Invoke();
            }
        }
    }
}