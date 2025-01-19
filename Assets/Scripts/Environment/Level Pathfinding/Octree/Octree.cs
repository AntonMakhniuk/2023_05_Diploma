using System.Collections.Generic;
using Environment.Level_Pathfinding.Level_Boundary;
using UnityEngine;

namespace Environment.Level_Pathfinding.Octree
{
    public class Octree
    {
        public OctreeNode RootNode;
        public Bounds Bounds;

        public Octree(HashSet<OctreeObject> objects, float minNodeSize)
        {
            CalculateBounds();
            CreateTree(objects, minNodeSize);
        }

        private void CreateTree(HashSet<OctreeObject> objects, float minNodeSize)
        {
            RootNode = new OctreeNode(Bounds, minNodeSize);

            foreach (var octreeObject in objects)
            {
                RootNode.Divide(octreeObject);
            }
        }

        private void CalculateBounds()
        {
            Bounds = LevelBoundaryController.Instance.GetWorldBounds();
        }
    }
}