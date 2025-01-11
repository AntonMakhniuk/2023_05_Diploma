using System.Collections.Generic;
using Scriptable_Object_Templates.Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public void CreateTree(HashSet<OctreeObject> objects, float minNodeSize)
        {
            RootNode = new OctreeNode(Bounds, minNodeSize);

            foreach (var octreeObject in objects)
            {
                RootNode.Divide(octreeObject);
            }
        }

        private void CalculateBounds()
        {
            var radius = LevelBoundaryDictionary.Instance
                    .dictionary[SceneManager.GetActiveScene().name]
                    .zone3Radius;
            
            Bounds.extents = new Vector3(radius, radius, radius);
        }
    }
}