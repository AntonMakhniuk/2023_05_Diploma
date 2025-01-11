using System;
using UnityEngine;

namespace Environment.Level_Pathfinding.Octree
{
    public class OctreeObject : MonoBehaviour
    {
        private static int _nextId;
        
        [HideInInspector] 
        public int id;
        
        private Bounds _bounds;

        private void Awake()
        {
            id = _nextId++;
            _bounds = gameObject.GetComponent<Collider>().bounds;
            
            if (OctreeGenerator.Instance == null)
            {
                OctreeGenerator.OnInstantiated += RegisterObject;
            }
            else
            {
                RegisterObject(null, null);    
            }   
        }

        private void RegisterObject(object sender, EventArgs e)
        {
            OctreeGenerator.Instance.AddObject(this);
            OctreeGenerator.OnInstantiated -= RegisterObject;
        }

        public bool Intersects(Bounds boundsToCheck)
        {
            return _bounds.Intersects(boundsToCheck);
        }

        private void OnDestroy()
        {
            OctreeGenerator.Instance.RemoveObject(this);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as OctreeObject);
        }

        private bool Equals(OctreeObject other)
        {
            return other != null &&
                   id == other.id;
        }

        private void OnValidate()
        {
            Awake();
        }
    }
}