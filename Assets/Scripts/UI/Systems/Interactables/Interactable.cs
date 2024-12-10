using System.Collections.Generic;
using Player;
using Player.Movement.Miscellaneous;
using UI.Systems.Interactables.States;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Systems.Interactables
{
    public class Interactable : MonoBehaviour
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
        
        private Dictionary<InteractableState, BaseInteractableState> _stateDictionary = new();
        private BaseInteractableState _currentState;
        
        private void Awake()
        {
            _stateDictionary = new Dictionary<InteractableState, BaseInteractableState>
            {
                { InteractableState.Hidden, new HiddenState(this) },
                { InteractableState.Shown, new ShownState(this) },
                { InteractableState.Highlit, new HighlitState(this) },
                { InteractableState.Activated, new ActivatedState(this) }
            };

            PlayerActions.InputActions.UI.InteractWithObject.performed += ToggleActivated;
            
            SetState(InteractableState.Hidden);
        }

        public void HandleCameraLooking()
        {
            if (_currentState is ShownState)
            {
                SetState(InteractableState.Highlit);
            }
        }
        
        private void SetState(InteractableState newState)
        {
            _currentState.Exit();
            _currentState = _stateDictionary[newState];
            _currentState.Enter();
        }
        
        private void ToggleActivated(InputAction.CallbackContext _)
        {
            switch (_currentState)
            {
                case HighlitState:
                {
                    SetState(InteractableState.Activated);
                    
                    break;
                }
                case ActivatedState:
                {
                    SetState(InteractableState.Highlit);
                    
                    break;
                }
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
            
            SetState(InteractableState.Shown);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
            
            SetState(InteractableState.Hidden);
        }

        private void OnDestroy()
        {
            PlayerActions.InputActions.UI.InteractWithObject.performed -= ToggleActivated;
        }
    }
}