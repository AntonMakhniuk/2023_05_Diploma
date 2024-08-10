using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Miscellaneous.Utility
{
    public static class RandomPointSelector
    {
        public static List<Vector3> GenerateRandomPointsOnMesh(Mesh targetMesh, int numOfPoints, float minDistance)
        {
            var selectedPoints = new List<Vector3>();
            var vertices = targetMesh.vertices;
            var triangles = targetMesh.triangles;
            
            for (int i = 0; i < numOfPoints; i++)
            {
                var pointCoords = GetRandomPointOnMesh(vertices, triangles);

                while (!IsPointFarEnough(pointCoords, selectedPoints, minDistance))
                {
                    pointCoords = GetRandomPointOnMesh(vertices, triangles);
                }
                
                selectedPoints.Add(pointCoords);
            }

            return selectedPoints;
        }

        private static Vector3 GetRandomPointOnMesh(Vector3[] vertices, int[] triangles)
        {
            var randomTriangleIndex = Random.Range(0, triangles.Length / 3) * 3;
            
            // The three vertices of a random triangle
            var vert1 = vertices[triangles[randomTriangleIndex]];
            var vert2 = vertices[triangles[randomTriangleIndex + 1]];
            var vert3 = vertices[triangles[randomTriangleIndex + 2]];

            // Barycentric coordinate weights, with the third being implicit
            var weight1 = Random.value;
            var weight2 = Random.value;

            // Inverting the weights if they are outside the triangle
            if (weight1 + weight2 > 1)
            {
                weight1 = 1 - weight1;
                weight2 = 1 - weight2;
            }

            return vert1 + weight1 * (vert2 - vert1) + weight2 * (vert3 - vert1);
        }
        
        private static bool IsPointFarEnough(Vector3 point, IEnumerable<Vector3> selectedPoints, float minDistance)
        {
            return selectedPoints.All(selectedPoint => 
                !(Vector3.Distance(point, selectedPoint) < minDistance));
        }
    }
}
