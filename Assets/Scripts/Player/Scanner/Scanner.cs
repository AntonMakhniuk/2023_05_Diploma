using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scanner
{
    public class Scanner : MonoBehaviour
    {
        [Header("Bounds Data")]
        [SerializeField] private GameObject scanBounds;
        [SerializeField] private float boundsExpansionTime = 2.5f;

        private bool _isScanning;
        private Sequence _currentSequence;
        private Vector3 _initialScale;

        private void Awake()
        {
            _initialScale = scanBounds.transform.localScale;
        }

        private void Start()
        {
            PlayerActions.InputActions.PlayerShip.ToggleScanner.performed += Toggle;
        }

        private void Toggle(InputAction.CallbackContext callbackContext)
        {
            _isScanning = !_isScanning;
            _currentSequence?.Kill();
            
            if (_isScanning)
            {
                scanBounds.transform.localScale = new Vector3(0,0,0);
                scanBounds.SetActive(true);
                _currentSequence.Append(scanBounds.transform.DOScale(_initialScale, boundsExpansionTime));
            }
            else
            {
                var localScale = scanBounds.transform.localScale;
                var averageScale = localScale.magnitude / _initialScale.magnitude;
                
                _currentSequence.Append(scanBounds.transform
                    .DOScale(0, boundsExpansionTime * averageScale)
                    .OnComplete(() => { scanBounds.SetActive(false); }));
            }
        }

        private void OnDestroy()
        {
            PlayerActions.InputActions.PlayerShip.ToggleScanner.performed -= Toggle;
        }
    }
}