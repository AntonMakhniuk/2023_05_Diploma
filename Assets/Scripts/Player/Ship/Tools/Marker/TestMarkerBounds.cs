using NaughtyAttributes;
using UnityEngine;

namespace Player.Ship.Tools.Marker
{
    public class TestMarkerBounds : MonoBehaviour
    {
        [Foldout("Color Options")] [SerializeField]
        private Color testNegativeColor;
        [Foldout("Color Options")] [SerializeField]
        private Color testPositiveColor;
        [Foldout("Color Options")] [SerializeField]
        private Color testNegativeEmissionColor;
        [Foldout("Color Options")] [SerializeField]
        private Color testPositiveEmissionColor;
        
        [ReadOnly] public int collectablesInside;
        
        private Material _boundsMaterial;
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        public bool IsValid => collectablesInside > 0;

        private void Start()
        {
            _boundsMaterial = GetComponent<MeshRenderer>().material;
            _boundsMaterial.SetColor(BaseColor, testNegativeColor);
            _boundsMaterial.SetColor(EmissionColor, testNegativeEmissionColor);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Collectable>(out var collectable) || collectable.isMarked)
            {
                return;
            }
            
            collectablesInside++;

            if (collectablesInside == 1)
            {
                _boundsMaterial.SetColor(BaseColor, testPositiveColor);
                _boundsMaterial.SetColor(EmissionColor, testPositiveEmissionColor);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Collectable>(out var collectable) || !collectable.isMarked)
            {
                return;
            }
            
            collectablesInside--;

            if (collectablesInside == 0)
            {
                _boundsMaterial.SetColor(BaseColor, testNegativeColor);
                _boundsMaterial.SetColor(EmissionColor, testNegativeEmissionColor);
            }
        }
        
        private void OnDisable()
        {
            collectablesInside = 0;
            
            _boundsMaterial.SetColor(BaseColor, testNegativeColor);
            _boundsMaterial.SetColor(EmissionColor, testNegativeEmissionColor);
        }
    }
}