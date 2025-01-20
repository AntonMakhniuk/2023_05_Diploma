using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Environment.Level_Pathfinding.Octree
{
    public class OctreeNode
    {
        private static int nextId;

        public OctreeNode[] ChildrenNodes;
        public readonly HashSet<OctreeObject> InnerObjects = new();
        public readonly int Id;
        public Bounds Bounds;
        public bool IsLeaf => ChildrenNodes == null;

        private readonly Bounds[] _childrenBounds = new Bounds[8];
        private float _minNodeSize;

        public OctreeNode(Bounds bounds, float minNodeSize)
        {
            Id = nextId++;
            Bounds = bounds;
            _minNodeSize = minNodeSize;

            var childSize = bounds.size * 0.5f;
            var centerOffset = bounds.size * 0.25f;

            for (var i = 0; i < 8; i++)
            {
                var childCenter = bounds.center;

                childCenter.x += centerOffset.x * ((i & 1) == 0 ? -1 : 1);
                childCenter.y += centerOffset.y * ((i & 2) == 0 ? -1 : 1);
                childCenter.z += centerOffset.z * ((i & 4) == 0 ? -1 : 1);

                _childrenBounds[i] = new Bounds(childCenter, childSize);
            }
        }

        public void Divide(OctreeObject octreeObject)
        {
            if (Bounds.size.x < _minNodeSize)
            {
                AddObject(octreeObject);
                
                return;
            }

            ChildrenNodes ??= new OctreeNode[8];

            var intersectedChild = false;

            for (var i = 0; i < 8; i++)
            {
                ChildrenNodes[i] ??= new OctreeNode(_childrenBounds[i], _minNodeSize);

                if (!octreeObject.Intersects(_childrenBounds[i]))
                {
                    continue;
                }
                
                ChildrenNodes[i].Divide(octreeObject);

                intersectedChild = true;
            }

            if (!intersectedChild)
            {
                AddObject(octreeObject);
            }
        }

        private void AddObject(OctreeObject octreeObject) => InnerObjects.Add(octreeObject);
        
        public void DrawNode()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);

            foreach (var _ in InnerObjects.Where(octreeObject => octreeObject.Intersects(Bounds)))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(Bounds.center, Bounds.size);
            }
            
            if (ChildrenNodes == null)
            {
                return;
            }
            
            foreach (var child in ChildrenNodes)
            {
                child?.DrawNode();
            }
        }
    }
}