using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Miscellaneous.Utility
{
    public static class RandomPointSelector
    {
        public static List<(Vector3 position, Quaternion rotation)> 
            GenerateRandomPointsOnMesh(Mesh targetMesh, int numOfPoints, float minDistance)
        {
            var selectedPoints = new List<(Vector3 position, Quaternion rotation)>();
            var vertices = targetMesh.vertices;
            var triangles = targetMesh.triangles;

            for (var i = 0; i < numOfPoints; i++)
            {
                Vector3 pointCoords;

                do
                {
                    pointCoords = GetRandomPointOnMesh(vertices, triangles);
                } 
                while (!IsPointFarEnough(pointCoords, selectedPoints
                           .Select(p => p.position).ToList(), minDistance));

                var center = vertices
                    .Aggregate(Vector3.zero, (current, vertex) => current + vertex);
                center /= vertices.Length;
                
                var centerToPosition = (pointCoords - center).normalized;
                var tangent = Vector3.Cross(centerToPosition, Vector3.up).normalized;
                var rotation = Quaternion.LookRotation(tangent, centerToPosition);
                
                selectedPoints.Add((pointCoords, rotation));
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

            var rand1 = Mathf.Sqrt(Random.value);
            var rand2 = Random.value;
            var randomPoint = (1 - rand1) * vert1 + rand1 * (1 - rand2) * vert2 + rand1 * rand2 * vert3;
            
            return randomPoint;
        }
        
        private static bool IsPointFarEnough(Vector3 point, IEnumerable<Vector3> selectedPoints, float minDistance)
        {
            return selectedPoints.All(selectedPoint => 
                !(Vector3.Distance(point, selectedPoint) < minDistance));
        }
    }
}
