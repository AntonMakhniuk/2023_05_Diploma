using System.Collections;
using Player;
using Player.Movement.Miscellaneous;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Mining.Tools.Base_Tools 
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
            PlayerActions.InputActions.PlayerShip.Primary.started += PrimaryActionStarted;
            PlayerActions.InputActions.PlayerShip.Primary.performed += PrimaryActionPerformed;
            PlayerActions.InputActions.PlayerShip.Primary.canceled += PrimaryActionCanceled;
            PlayerActions.InputActions.PlayerShip.Secondary.started += SecondaryActionStarted;
            PlayerActions.InputActions.PlayerShip.Secondary.performed += SecondaryActionPerformed;
            PlayerActions.InputActions.PlayerShip.Secondary.canceled += SecondaryActionCanceled;
            PlayerActions.InputActions.PlayerShip.Tetriary.started += TetriaryActionStarted;
            PlayerActions.InputActions.PlayerShip.Tetriary.performed += TetriaryActionPerformed;
            PlayerActions.InputActions.PlayerShip.Tetriary.canceled += TetriaryActionCanceled;
            PlayerActions.InputActions.PlayerShip.Scroll.started += ScrollActionStarted;
            PlayerActions.InputActions.PlayerShip.Scroll.performed += ScrollActionPerformed;
            PlayerActions.InputActions.PlayerShip.Scroll.canceled += ScrollActionCanceled;

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
        
        private void TetriaryActionStarted(InputAction.CallbackContext _)
        {
            if (_isActiveTool)
            {
                TetriaryActionStarted();
            }
        }

        protected abstract void TetriaryActionStarted();
        
        private void TetriaryActionPerformed(InputAction.CallbackContext _)
        {
            if (_isActiveTool)
            {
                TetriaryActionPerformed();
            }
        }
        
        protected abstract void TetriaryActionPerformed();
        
        private void TetriaryActionCanceled(InputAction.CallbackContext _)
        {
            if (_isActiveTool)
            {
                TetriaryActionCanceled();
            }
        }

        protected abstract void TetriaryActionCanceled();
        
        private void ScrollActionStarted(InputAction.CallbackContext ctx)
        {
            if (!_isActiveTool)
            {
                return;
            }
            
            ScrollStarted(ctx);
        }
        
        protected abstract void ScrollStarted(InputAction.CallbackContext ctx);

        private void ScrollActionPerformed(InputAction.CallbackContext ctx)
        {
            if (!_isActiveTool)
            {
                return;
            }
            
            ScrollPerformed(ctx);
        }

        protected abstract void ScrollPerformed(InputAction.CallbackContext ctx);

        private void ScrollActionCanceled(InputAction.CallbackContext ctx)
        {
            if (!_isActiveTool)
            {
                return;
            }
            
            ScrollCanceled(ctx);
        }
        
        protected abstract void ScrollCanceled(InputAction.CallbackContext ctx);
        
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
            
            OnActivate();
        }

        protected abstract void OnActivate();

        private void Deactivate()
        {
            if (!_isWorking)
            {
                return;
            }

            OnDeactivate();
            
            StopCoroutine(_workCoroutine);
            StopCoroutine(_fixedWorkCoroutine);
            _isActiveTool = false;
            _isWorking = false;
        }

        protected abstract void OnDeactivate();
        
        protected virtual void OnDestroy()
        {
            PlayerActions.InputActions.PlayerShip.Primary.started -= PrimaryActionStarted;
            PlayerActions.InputActions.PlayerShip.Primary.performed -= PrimaryActionPerformed;
            PlayerActions.InputActions.PlayerShip.Primary.canceled -= PrimaryActionCanceled;
            PlayerActions.InputActions.PlayerShip.Secondary.started -= SecondaryActionStarted;
            PlayerActions.InputActions.PlayerShip.Secondary.performed -= SecondaryActionPerformed;
            PlayerActions.InputActions.PlayerShip.Secondary.canceled -= SecondaryActionCanceled;
            PlayerActions.InputActions.PlayerShip.Tetriary.started -= TetriaryActionStarted;
            PlayerActions.InputActions.PlayerShip.Tetriary.performed -= TetriaryActionPerformed;
            PlayerActions.InputActions.PlayerShip.Tetriary.canceled -= TetriaryActionCanceled;
            PlayerActions.InputActions.PlayerShip.Scroll.started -= ScrollActionStarted;
            PlayerActions.InputActions.PlayerShip.Scroll.performed -= ScrollActionPerformed;
            PlayerActions.InputActions.PlayerShip.Scroll.canceled -= ScrollActionCanceled;
            
            StopAllCoroutines();
        }
    }

    public enum ToolType
    {
        Laser, Bomb, Marker
    }
}