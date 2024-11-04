using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Uses code from https://github.com/MinaPecheux/unity-tutorials/blob/main/Assets/00-Shaders/CrossPlatformWireframe/Scripts/MeshWireframeComputor.cs

namespace Miscellaneous
{
    [RequireComponent(typeof(MeshFilter))]
    public class MeshWireframeController : MonoBehaviour
    {
        private static readonly Color[] Colors = 
        {
            new(1, 0, 0, 0),
            new(0, 1, 0, 0), 
            new(0, 0, 1, 0),
            new(0, 0, 0, 1) 
        };

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateMesh();
        }
#endif

        [ContextMenu("Update Mesh")]
        public void UpdateMesh()
        {
            if (!gameObject.activeSelf || !GetComponent<MeshRenderer>().enabled)
            {
                return;
            }

            var m = GetComponent<MeshFilter>().sharedMesh;
            
            if (m == null)
            {
                return;
            }
            
            var colors = _SortedColoring(m);

            if (colors != null)
            {
                m.SetColors(colors);
            }
        }

        private Color[] _SortedColoring(Mesh mesh)
        {
            var n = mesh.vertexCount;
            var labels = new int[n];
            
            var triangles = _GetSortedTriangles(mesh.triangles);
            triangles.Sort((t1, t2) =>
            {
                var i = 0;
                
                while (i < t1.Length && i < t2.Length)
                {
                    if (t1[i] < t2[i])
                    {
                        return -1;
                    }
                    
                    if (t1[i] > t2[i])
                    {
                        return 1;
                    }
                    
                    i += 1;
                }
                
                if (t1.Length < t2.Length)
                {
                    return -1;
                }
                
                if (t1.Length > t2.Length)
                {
                    return 1;
                }
                
                return 0;
            });
            
            foreach (var triangle in triangles)
            {
                var availableLabels = new List<int> { 1, 2, 3, 4 };
                
                foreach (var vertexIndex in triangle)
                {
                    if (availableLabels.Contains(labels[vertexIndex]))
                    {
                        availableLabels.Remove(labels[vertexIndex]);
                    }
                }
                
                foreach (var vertexIndex in triangle)
                {
                    if (labels[vertexIndex] != 0)
                    {
                        continue;
                    }
                    
                    if (availableLabels.Count == 0)
                    {
                        Debug.LogError("Could not find color");
                        
                        return null;
                    }
                    
                    labels[vertexIndex] = availableLabels[0];
                    availableLabels.RemoveAt(0);
                }
            }
            
            var colors = new Color[n];
            
            for (var i = 0; i < n; i++)
            {
                colors[i] = labels[i] > 0 ? Colors[labels[i] - 1] : Colors[0];
            }

            return colors;
        }

        private List<int[]> _GetSortedTriangles(int[] triangles)
        {
            var result = new List<int[]>();
            
            for (var i = 0; i < triangles.Length; i += 3)
            {
                var t = new List<int> { triangles[i], triangles[i + 1], triangles[i + 2] };
                
                t.Sort();
                result.Add(t.ToArray());
            }
            
            return result;
        }

        [Header("Wireframe settings")] 
        [SerializeField] private Material wireframeMaterial;
        private static readonly int WireframeThickness = Shader.PropertyToID("_Wireframe_Thickness");
        [SerializeField] private float wireframeThickness = 0.3f;
        [SerializeField] private float stateChangeSpeed = 0.5f;

        [Header("Pulse settings")]
        [SerializeField] private bool isPulsating;
        [SerializeField] private float minWireframeThickness = 0.1f;
        [SerializeField] private float pulseInterval = 1f;

        private float _currentThickness;
        private Coroutine _stateChangeCoroutine, _pulseCoroutine;

        private void Start()
        {
            wireframeMaterial.SetFloat(WireframeThickness, 0);
        }

        public void EnableWireframe()
        {
            if (_stateChangeCoroutine != null)
            {
                StopCoroutine(_stateChangeCoroutine);
            }

            _stateChangeCoroutine = StartCoroutine(EnableWireframeCoroutine());
        }
        
        private IEnumerator EnableWireframeCoroutine()
        {
            wireframeMaterial.SetFloat(WireframeThickness, _currentThickness);
            
            var initialThickness = _currentThickness;
            var changeTime = stateChangeSpeed * 
                             Mathf.Clamp01((wireframeThickness - initialThickness) / wireframeThickness);
            var elapsedTime = 0f;

            while (_currentThickness < wireframeThickness)
            {
                elapsedTime += Time.deltaTime;
                _currentThickness = Mathf.Lerp(initialThickness, wireframeThickness, elapsedTime / changeTime);
                wireframeMaterial.SetFloat(WireframeThickness, _currentThickness);
                
                yield return null;
            }
            
            _currentThickness = wireframeThickness;
            wireframeMaterial.SetFloat(WireframeThickness, _currentThickness);

            if (isPulsating)
            {
                _pulseCoroutine = StartCoroutine(PulseCoroutine());
            }
        }

        private IEnumerator PulseCoroutine()
        {
            while (true)
            {
                var elapsedTime = 0f;
                
                while (elapsedTime < pulseInterval / 2f)
                {
                    elapsedTime += Time.deltaTime;
                    
                    _currentThickness = 
                        Mathf.Lerp(wireframeThickness, minWireframeThickness, elapsedTime / (pulseInterval / 2f));
                    wireframeMaterial.SetFloat(WireframeThickness, _currentThickness);
                    
                    yield return null;
                }

                elapsedTime = 0f;
                
                while (elapsedTime < pulseInterval / 2f)
                {
                    elapsedTime += Time.deltaTime;
                    
                    _currentThickness = 
                        Mathf.Lerp(minWireframeThickness, wireframeThickness, elapsedTime / (pulseInterval / 2f));
                    wireframeMaterial.SetFloat(WireframeThickness, _currentThickness);
                    
                    yield return null;
                }
            }
        }

        public void DisableWireframe()
        {
            if (_stateChangeCoroutine != null)
            {
                StopCoroutine(_stateChangeCoroutine);
            }
            
            if (_pulseCoroutine != null)
            {
                StopCoroutine(_pulseCoroutine);    
            }

            _stateChangeCoroutine = StartCoroutine(DisableWireframeCoroutine());
        }
        
        private IEnumerator DisableWireframeCoroutine()
        {
            wireframeMaterial.SetFloat(WireframeThickness, _currentThickness);
            
            //TODO: Double-check this math cause its a bit sussy ngl
            var initialThickness = _currentThickness;
            var changeTime = stateChangeSpeed * Mathf.Clamp01(initialThickness / wireframeThickness);
            var elapsedTime = 0f;

            while (_currentThickness > 0f)
            {
                elapsedTime += Time.deltaTime;
                
                _currentThickness = Mathf.Lerp(initialThickness, 0, elapsedTime / changeTime);
                wireframeMaterial.SetFloat(WireframeThickness, _currentThickness);
                
                yield return null;
            }

            _currentThickness = 0f;
            wireframeMaterial.SetFloat(WireframeThickness, _currentThickness);
        }
    }
}
