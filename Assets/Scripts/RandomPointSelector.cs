using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomPointSelector : MonoBehaviour
{
    [SerializeField] private MeshCollider lookupCollider;
    [SerializeField] private GameObject asteroidPointPrefab; // Prefab for the asteroid point

    private Vector3[] points = new Vector3[3];
    private List<GameObject> childPoints = new List<GameObject>();

    [SerializeField] private int startCounter = 0;

    private void Start() {
        startCounter++;
        DrawThreeRandomPoints();
    }

    private void Update()
    {
        // Your update logic here if needed
    }

    public void OnDrawGizmos()
    {
        // Use Gizmos for visualization in the Scene view if needed
         foreach (var childPoint in childPoints)
         {
             Gizmos.color = Color.yellow; // Change color as needed
             Gizmos.DrawSphere(childPoint.transform.position, 0.1f);
         }
    }

    private void DrawThreeRandomPoints()
    {
        points[0] = GetRandomPointOnMesh(lookupCollider.sharedMesh) + lookupCollider.transform.position;
        points[1] = GetRandomPointOnMesh(lookupCollider.sharedMesh) + lookupCollider.transform.position;
        points[2] = GetRandomPointOnMesh(lookupCollider.sharedMesh) + lookupCollider.transform.position;

        float[] scaleValues = new float[] { transform.localScale.x, transform.localScale.y, transform.localScale.z };

        // Your validation logic for point distances here
        while (Vector3.Distance(points[0], points[1]) <= 0.6 * Mathf.Min(scaleValues))
                    points[1] = GetRandomPointOnMesh(lookupCollider.sharedMesh) + lookupCollider.transform.position;
        
        while (Vector3.Distance(points[0], points[2]) <= 0.6 * Mathf.Min(scaleValues) || 
               Vector3.Distance(points[1], points[2]) <= 0.6 * Mathf.Min(scaleValues))
                    points[2] = GetRandomPointOnMesh(lookupCollider.sharedMesh) + lookupCollider.transform.position;

        for (int i = 0; i < points.Length; i++)
        {
            GameObject asteroidPoint = Instantiate(asteroidPointPrefab, points[i], Quaternion.identity);
            asteroidPoint.name = "AsteroidPoint" + i;
            asteroidPoint.transform.parent = transform;
            childPoints.Add(asteroidPoint);
        }
    }

    private Vector3 GetRandomPointOnMesh(Mesh mesh)
    {
        // Your point generation logic here

        float[] sizes = GetTriSizes(mesh.triangles, mesh.vertices);
        float[] cumulativeSizes = new float[sizes.Length];
        float total = 0;

        for (int i = 0; i < sizes.Length; i++)
        {
            total += sizes[i];
            cumulativeSizes[i] = total;
        }

        float randomsample = Random.value * total;

        int triIndex = -1;

        for (int i = 0; i < sizes.Length; i++)
        {
            if (randomsample <= cumulativeSizes[i])
            {
                triIndex = i;
                break;
            }
        }

        if (triIndex == -1) Debug.LogError("triIndex should never be -1");

        Vector3 a = mesh.vertices[mesh.triangles[triIndex * 3]];
        Vector3 b = mesh.vertices[mesh.triangles[triIndex * 3 + 1]];
        Vector3 c = mesh.vertices[mesh.triangles[triIndex * 3 + 2]];

        float r = Random.value;
        float s = Random.value;

        if (r + s >= 1)
        {
            r = 1 - r;
            s = 1 - s;
        }

        Vector3 pointOnMesh = a + r * (b - a) + s * (c - a);
        pointOnMesh.Scale(transform.localScale);
        return pointOnMesh;
    }

    private float[] GetTriSizes(int[] tris, Vector3[] verts)
    {
        int triCount = tris.Length / 3;
        float[] sizes = new float[triCount];
        for (int i = 0; i < triCount; i++)
        {
            sizes[i] = 0.5f * Vector3.Cross(verts[tris[i * 3 + 1]] - verts[tris[i * 3]], verts[tris[i * 3 + 2]] - verts[tris[i * 3]]).magnitude;
        }
        return sizes;
    }

    public List<GameObject> GetAsteroidPoints()
    {
        return childPoints;
    }
}
