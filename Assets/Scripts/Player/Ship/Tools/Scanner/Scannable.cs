using System.Linq;
using DG.Tweening;
using Miscellaneous;
using Scriptable_Object_Templates.Singletons;
using UnityEngine;

namespace Player.Ship.Tools.Scanner
{
    public class Scannable : MonoBehaviour
    {
        private const float TransparencyPercent = 0.5f;
        private const float AlphaChangeSpeed = 0.5f;
        
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
                    
                    for (var i = 0; i < materials.Count; i++)
                    {
                        if (!materials[i].name.Contains("Opaque"))
                        {
                            continue;
                        }
                        
                        materials[i] = MaterialAlphaDictionary.Instance.OpaqueToTransparent(materials[i]);
                        
                        DOTween.Kill(materials[i].color);
                        
                        var color = materials[i].color;
                        color.a = TransparencyPercent;

                        var index = i;
                        
                        materials[i].DOColor(color, AlphaChangeSpeed).OnKill(() =>
                        {
                            materials[index] = 
                                MaterialAlphaDictionary.Instance.OpaqueToTransparent(materials[index]);
                        });
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
                    
                    for (var i = 0; i < materials.Count; i++)
                    {
                        if (!materials[i].name.Contains("Transparent"))
                        {
                            continue;
                        }
                        
                        DOTween.Kill(materials[i].color);
                        
                        var color = materials[i].color;
                        color.a = 1f;
                        materials[i].color = color;

                        var index = i;
                        materials[i].DOColor(color, AlphaChangeSpeed)
                            .OnComplete(() =>
                            {
                                materials[index] = 
                                    MaterialAlphaDictionary.Instance.TransparentToOpaque(materials[index]);
                            })
                            .OnKill(() =>
                            {
                                materials[index] = 
                                    MaterialAlphaDictionary.Instance.TransparentToOpaque(materials[index]);
                            });
                    }
                    
                    _targetRenderer.materials = materials.ToArray();
                }
                
                _wireframeController.DisableWireframe();
            }
        }
    }
}