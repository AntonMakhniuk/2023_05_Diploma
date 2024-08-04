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
            PlayerActions.InputActions.PlayerShip.ToolPrimary.performed += PrimaryAction;
            PlayerActions.InputActions.PlayerShip.ToolSecondary.performed += SecondaryAction;

            _workCoroutine = WorkCoroutine();
        }

        protected abstract IEnumerator WorkCoroutine();
        
        private void PrimaryAction(InputAction.CallbackContext _)
        {
            if (_isActiveTool)
            {
                PrimaryAction();
            }
        }
        
        protected abstract void PrimaryAction();
        
        private void SecondaryAction(InputAction.CallbackContext _)
        {
            if (_isActiveTool)
            {
                SecondaryAction();
            }
        }
        
        protected abstract void SecondaryAction();

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
            PlayerActions.InputActions.PlayerShip.ToolPrimary.performed -= PrimaryAction;
            PlayerActions.InputActions.PlayerShip.ToolSecondary.performed -= SecondaryAction;
            
            StopAllCoroutines();
        }
    }
}