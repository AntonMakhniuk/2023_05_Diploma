using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Ship.Tools.Scanner
{
    public class Scanner : MonoBehaviour
    {
        [Foldout("Bounds Data")]
        [SerializeField] private GameObject scanBounds;
        [Foldout("Bounds Data")] 
        [SerializeField] private float boundsRadius = 250;
        [Foldout("Bounds Data")] 
        [SerializeField] private float baseMaskScale = 20;
        [Foldout("Bounds Data")]
        [SerializeField] private float boundsExpansionTime = 2.5f;
        
        private static readonly int MaskScale = Shader.PropertyToID("_Mask_Scale");
        
        private bool _isScanning;
        private Vector3 _initialScale;
        private Material _boundsMaterial;
        private float _maskScale;

        private void Awake()
        {
            _initialScale = new Vector3(boundsRadius, boundsRadius, boundsRadius);
            _boundsMaterial = scanBounds.GetComponent<MeshRenderer>().material;
            _maskScale = baseMaskScale * boundsRadius / 100;
            _boundsMaterial.SetVector(MaskScale, 
                new Vector4(_maskScale, _maskScale, _maskScale, _maskScale));
        }

        private void Start()
        {
            PlayerActions.InputActions.PlayerShip.ToggleScanner.performed += Toggle;
        }

        private void Toggle(InputAction.CallbackContext callbackContext)
        {
            _isScanning = !_isScanning;

            DOTween.Kill(scanBounds.transform);
            
            if (_isScanning)
            {
                scanBounds.transform.localScale = new Vector3(0,0,0);
                scanBounds.SetActive(true);
                scanBounds.transform
                    .DOScale(_initialScale, boundsExpansionTime)
                    .OnUpdate(() =>
                    {
                        _maskScale = baseMaskScale * scanBounds.transform.localScale.x / 100;
                        _boundsMaterial.SetVector(MaskScale, 
                            new Vector4(_maskScale, _maskScale, _maskScale, _maskScale));
                    });
            }
            else
            {
                var localScale = scanBounds.transform.localScale;
                var averageScale = localScale.magnitude / _initialScale.magnitude;
                
                scanBounds.transform
                    .DOScale(0, boundsExpansionTime * averageScale)
                    .OnUpdate(() =>
                    {
                        _maskScale = baseMaskScale * scanBounds.transform.localScale.x / 100;
                        _boundsMaterial.SetVector(MaskScale, 
                            new Vector4(_maskScale, _maskScale, _maskScale, _maskScale));
                    })
                    .OnComplete(() => { scanBounds.SetActive(false); });
            }
        }

        private void OnDestroy()
        {
            PlayerActions.InputActions.PlayerShip.ToggleScanner.performed -= Toggle;
        }
    }
}