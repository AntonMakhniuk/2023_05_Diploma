using DG.Tweening;
using NaughtyAttributes;
using Player.Movement.Miscellaneous;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Ship.Tools.Scanner
{
    public class Scanner : MonoBehaviour
    {
        [Foldout("Bounds Data")] [SerializeField] 
        private GameObject scanBounds;
        [Foldout("Bounds Data")] [SerializeField] 
        private float boundsRadius = 250;
        [Foldout("Bounds Data")] [SerializeField] 
        private float baseMaskScale = 20;
        [Foldout("Bounds Data")] [SerializeField] 
        private float boundsExpansionTime = 2.5f;
        [Foldout("Bounds Data")] [MinMaxSlider(0f, 100f)] [SerializeField] 
        private Vector2 opacityPulseRange = new Vector2(2.5f, 5f);
        [Foldout("Bounds Data")] [SerializeField]
        private float pulseInterval = 5f;
        
        [Foldout("Vignette Data")]
        [SerializeField] private Material vignetteMaterial;
        
        private static readonly int MaskScale = Shader.PropertyToID("_Mask_Scale");
        private static readonly int VignetteAlpha = Shader.PropertyToID("_Alpha");
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");
        
        private bool _isScanning;
        private Vector3 _initialScale;
        private Material _boundsMaterial;
        private float _maskScale;

        private void Awake()
        {
            _initialScale = new Vector3(boundsRadius, boundsRadius, boundsRadius);
            
            Debug.Log(scanBounds);
            
            _boundsMaterial = scanBounds.GetComponent<MeshRenderer>().material;
            _maskScale = baseMaskScale * boundsRadius / 100;
            _boundsMaterial.SetVector(MaskScale, 
                new Vector4(_maskScale, _maskScale, _maskScale, _maskScale));
            _boundsMaterial.SetFloat(Opacity, opacityPulseRange.x);
            vignetteMaterial.SetFloat(VignetteAlpha, 0f);
        }

        private void Start()
        {
            PlayerActions.InputActions.PlayerShip.ToggleScanner.performed += Toggle;
            
            _boundsMaterial
                .DOFloat(opacityPulseRange.y, Opacity, pulseInterval / 2)
                .SetLoops(-1, LoopType.Yoyo)
                .SetAutoKill(false);
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

                vignetteMaterial.DOFloat(1f, VignetteAlpha, boundsExpansionTime);
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
                
                vignetteMaterial.DOFloat(0f, VignetteAlpha, boundsExpansionTime * averageScale);
            }
        }

        private void OnDestroy()
        {
            PlayerActions.InputActions.PlayerShip.ToggleScanner.performed -= Toggle;
        }
    }
}