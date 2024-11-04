using System.Collections;
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

        private IEnumerator _workCoroutine;
        private IEnumerator _fixedWorkCoroutine;
        private bool _isWorking;

        protected virtual void Start()
        {
            PlayerActions.InputActions.PlayerShip.ToolPrimary.started += PrimaryActionStarted;
            PlayerActions.InputActions.PlayerShip.ToolPrimary.performed += PrimaryActionPerformed;
            PlayerActions.InputActions.PlayerShip.ToolPrimary.canceled += PrimaryActionCanceled;
            PlayerActions.InputActions.PlayerShip.ToolSecondary.started += SecondaryActionStarted;
            PlayerActions.InputActions.PlayerShip.ToolSecondary.performed += SecondaryActionPerformed;
            PlayerActions.InputActions.PlayerShip.ToolSecondary.canceled += SecondaryActionCanceled;
            PlayerActions.InputActions.PlayerShip.ToolThird.started += ThirdActionStarted;
            PlayerActions.InputActions.PlayerShip.ToolThird.performed += ThirdActionPerformed;
            PlayerActions.InputActions.PlayerShip.ToolThird.canceled += ThirdActionCanceled;

            IsActiveTool = false;
            _workCoroutine = WorkCoroutine();
            _fixedWorkCoroutine = FixedWorkCoroutine();
        }
        
        // Run akin to an Update, but only when the tool is active
        private IEnumerator WorkCoroutine()
        {
            while (true)
            {
                WorkCycle();

                yield return null;
            }
        }

        protected virtual void WorkCycle()
        {
            
        }
        
        // Run akin to a FixedUpdate, but only when the tool is active
        private IEnumerator FixedWorkCoroutine()
        {
            while (true)
            {
                FixedWorkCycle();

                yield return new WaitForFixedUpdate();
            }
        }

        protected virtual void FixedWorkCycle()
        {
            
        }
        
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
        
        private void ThirdActionStarted(InputAction.CallbackContext _)
        {
            if (_isActiveTool)
            {
                ThirdActionStarted();
            }
        }

        protected abstract void ThirdActionStarted();
        
        private void ThirdActionPerformed(InputAction.CallbackContext _)
        {
            if (_isActiveTool)
            {
                ThirdActionPerformed();
            }
        }
        
        protected abstract void ThirdActionPerformed();
        
        private void ThirdActionCanceled(InputAction.CallbackContext _)
        {
            if (_isActiveTool)
            {
                ThirdActionCanceled();
            }
        }

        protected abstract void ThirdActionCanceled();
        
        private void ToggleInstrument(bool newState)
        {
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
            if (_isWorking)
            {
                return;
            }
            
            StartCoroutine(_workCoroutine);
            StartCoroutine(_fixedWorkCoroutine);
            _isActiveTool = true;
            _isWorking = true;
        }

        private void Deactivate()
        {
            if (!_isWorking)
            {
                return;
            }
            
            StopCoroutine(_workCoroutine);
            StopCoroutine(_fixedWorkCoroutine);
            _isActiveTool = false;
            _isWorking = false;
        }
        
        protected virtual void OnDestroy()
        {
            PlayerActions.InputActions.PlayerShip.ToolPrimary.started -= PrimaryActionStarted;
            PlayerActions.InputActions.PlayerShip.ToolPrimary.performed -= PrimaryActionPerformed;
            PlayerActions.InputActions.PlayerShip.ToolPrimary.canceled -= PrimaryActionCanceled;
            PlayerActions.InputActions.PlayerShip.ToolSecondary.started -= SecondaryActionStarted;
            PlayerActions.InputActions.PlayerShip.ToolSecondary.performed -= SecondaryActionPerformed;
            PlayerActions.InputActions.PlayerShip.ToolSecondary.canceled -= SecondaryActionCanceled;
            PlayerActions.InputActions.PlayerShip.ToolThird.started -= ThirdActionStarted;
            PlayerActions.InputActions.PlayerShip.ToolThird.performed -= ThirdActionPerformed;
            PlayerActions.InputActions.PlayerShip.ToolThird.canceled -= ThirdActionCanceled;
            
            StopAllCoroutines();
        }
    }

    public enum ToolType
    {
        Laser, BombLauncher
    }
}