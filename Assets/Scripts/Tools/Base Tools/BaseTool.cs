using System.Collections;
using Cinemachine;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools.Base_Tools 
{
    public abstract class BaseTool : MonoBehaviour
    {
        private bool _isActiveTool;
        public bool IsActiveTool
        {
            get => _isActiveTool;
            set => ToggleInstrument(value);
        }
        
        [SerializeField] protected CinemachineVirtualCamera cinematicCamera;
        [SerializeField] protected Canvas crosshairCanvas;

        private IEnumerator _workCoroutine;

        private void Start()
        {
            PlayerActions.InputActions.PlayerShip.ToolPrimary.started += PrimaryActionStarted;
            PlayerActions.InputActions.PlayerShip.ToolPrimary.performed += PrimaryActionPerformed;
            PlayerActions.InputActions.PlayerShip.ToolPrimary.canceled += PrimaryActionCanceled;
            PlayerActions.InputActions.PlayerShip.ToolSecondary.started += SecondaryActionStarted;
            PlayerActions.InputActions.PlayerShip.ToolSecondary.performed += SecondaryActionPerformed;
            PlayerActions.InputActions.PlayerShip.ToolSecondary.canceled += SecondaryActionCanceled;

            _workCoroutine = WorkCoroutine();
        }
        
        protected abstract IEnumerator WorkCoroutine();
        
        private void PrimaryActionStarted(InputAction.CallbackContext _)
        {
            if (_isActiveTool)
            {
                PrimaryActionStarted();
            }
        }

        protected abstract void PrimaryActionStarted();
        
        private void PrimaryActionPerformed(InputAction.CallbackContext _)
        {
            if (_isActiveTool)
            {
                PrimaryActionPerformed();
            }
        }
        
        protected abstract void PrimaryActionPerformed();
        
        private void PrimaryActionCanceled(InputAction.CallbackContext _)
        {
            if (_isActiveTool)
            {
                PrimaryActionCanceled();
            }
        }

        protected abstract void PrimaryActionCanceled();
        
        private void SecondaryActionStarted(InputAction.CallbackContext _)
        {
            if (_isActiveTool)
            {
                SecondaryActionStarted();
            }
        }

        protected abstract void SecondaryActionStarted();
        
        private void SecondaryActionPerformed(InputAction.CallbackContext _)
        {
            if (_isActiveTool)
            {
                SecondaryActionPerformed();
            }
        }
        
        protected abstract void SecondaryActionPerformed();
        
        private void SecondaryActionCanceled(InputAction.CallbackContext _)
        {
            if (_isActiveTool)
            {
                SecondaryActionCanceled();
            }
        }

        protected abstract void SecondaryActionCanceled();
        
        private void ToggleInstrument(bool newState)
        {
            cinematicCamera.gameObject.SetActive(newState);
            crosshairCanvas.gameObject.SetActive(newState);

            if (newState)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

        private void Activate()
        {
            StartCoroutine(_workCoroutine);
        }

        private void Deactivate()
        {
            StopCoroutine(_workCoroutine);
        }
        
        protected virtual void OnDestroy()
        {
            PlayerActions.InputActions.PlayerShip.ToolPrimary.started -= PrimaryActionStarted;
            PlayerActions.InputActions.PlayerShip.ToolPrimary.performed -= PrimaryActionPerformed;
            PlayerActions.InputActions.PlayerShip.ToolPrimary.canceled -= PrimaryActionCanceled;
            PlayerActions.InputActions.PlayerShip.ToolSecondary.started -= SecondaryActionStarted;
            PlayerActions.InputActions.PlayerShip.ToolSecondary.performed -= SecondaryActionPerformed;
            PlayerActions.InputActions.PlayerShip.ToolSecondary.canceled -= SecondaryActionCanceled;
            
            StopAllCoroutines();
        }
    }
}