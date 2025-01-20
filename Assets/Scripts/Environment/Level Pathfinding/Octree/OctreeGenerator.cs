using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Environment.Level_Pathfinding.Octree
{
    public class OctreeGenerator : MonoBehaviour
    {
        public static OctreeGenerator Instance;
        
        [Foldout("Node Data")]
        public float minNodeSize = 1f;

        private readonly HashSet<OctreeObject> _objects = new();
        private Octree _octree;

        public static event EventHandler OnInstantiated; 
        
        private void Awake()
        {
            Instance = this;
            
            OnInstantiated?.Invoke(this, EventArgs.Empty);
            
            GenerateOctree();
        }

        public void AddObject(OctreeObject octreeObject)
        {
            _objects.Add(octreeObject);
        }

        public void RemoveObject(OctreeObject octreeObject)
        {
            _objects.Remove(octreeObject);
        }

        [Button("Debug: Generate Instance (first)", enabledMode: EButtonEnableMode.Editor)]
        private void GenerateInstance()
        {
            Awake();
        }
        
        [Button("Debug: Generate Octree (second)", enabledMode: EButtonEnableMode.Editor)]
        public void GenerateOctree()
        {
            Debug.Log(_objects.Count);
            
            _octree = new Octree(_objects, minNodeSize);

            if (!Application.isEditor)
            {
                return;
            }
            
            UnityEditor.SceneView.RepaintAll();
        }

        private void OnDrawGizmos()
        {
            if (_octree == null)
            {
                return;
            }
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_octree.Bounds.center, _octree.Bounds.size);
            
            _octree.RootNode.DrawNode();
        }
    }
}