using System.Linq;
using Miscellaneous;
using NaughtyAttributes;
using UnityEngine;

namespace Player.Scanner
{
    public class Scannable : MonoBehaviour
    {
        private const float TransparencyPercent = 0.5f;
        
        [SerializeField] private bool isValuable;

        private MeshRenderer _targetRenderer;
        private MeshWireframeController _wireframeController;
        private bool _isHighlit;

        private void Awake()
        {
             TryGetComponent(out _targetRenderer);
             TryGetComponent(out _wireframeController);

             if (_targetRenderer == null)
             {
                 Debug.LogError("Highlightable object " + this + " has no mesh renderer.");
             }
             if (_wireframeController == null)
             {
                 Debug.LogError("Highlightable object " + this + " has no wireframe controller.");
             }
        }

        public void Toggle()
        {
            if (_targetRenderer == null || _wireframeController == null)
            {
                return;
            }
            
            _isHighlit = !_isHighlit;

            if (_isHighlit)
            {
                if (!isValuable)
                {
                    var materials = _targetRenderer.materials.ToList();
                    
                    foreach (var material in materials)
                    {
                        var color = material.color;
                        color.a = TransparencyPercent;
                        material.color = color;
                    }
                    
                    _targetRenderer.materials = materials.ToArray();
                }
                
                _wireframeController.EnableWireframe();
            }
            else
            {
                if (!isValuable)
                {
                    var materials = _targetRenderer.materials.ToList();
                    
                    foreach (var material in materials)
                    {
                        var color = material.color;
                        color.a = 1f;
                        material.color = color;
                    }
                    
                    _targetRenderer.materials = materials.ToArray();
                }
                
                _wireframeController.DisableWireframe();
            }
        }
    }
}