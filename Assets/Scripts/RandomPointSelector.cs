using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class RandomPointSelector : MonoBehaviour
{
    [SerializeField] private MeshCollider lookupCollider;

    [SerializeField] private bool redrawPoints;
    private Vector3 randomPoint;

    private Color[] colors = new[] { Color.red, Color.green, Color.blue};
    private Vector3[] points = new Vector3[3];
    [SerializeField] private List<GameObject> childPoints = new List<GameObject>(); 

    private void Start() {
        DrawThreeRandomPoints();
    }

    void Update () {
        if (redrawPoints) {
            Array.Clear(points, 0, points.Length);
            DrawThreeRandomPoints();
            redrawPoints = false;
        }
	}
    
    public void OnDrawGizmos() {
        // foreach (Vector3 point in points) {
        //     int index = Array.IndexOf(points, point);
        //     Gizmos.color = colors[index];
        //     Gizmos.DrawSphere(point, 0.1f);
        // }

        foreach (var childPoint in childPoints) {
            Gizmos.DrawSphere(childPoint.transform.position, 0.1f);
        }
    }

    private void DrawThreeRandomPoints() {
        points[0] = GetRandomPointOnMesh(lookupCollider.sharedMesh) + lookupCollider.transform.position;
        
        points[1] = GetRandomPointOnMesh(lookupCollider.sharedMesh) + lookupCollider.transform.position;
        points[2] = GetRandomPointOnMesh(lookupCollider.sharedMesh) + lookupCollider.transform.position;
        
        float[] scaleValues = new float[] { transform.localScale.x, transform.localScale.y, transform.localScale.z };
        
        while (Vector3.Distance(points[0], points[1]) <= 0.7 * Mathf.Min(scaleValues))
            points[1] = GetRandomPointOnMesh(lookupCollider.sharedMesh) + lookupCollider.transform.position;
        
        // Debug.Log("0 and 1 distance: " + Vector3.Distance(points[0], points[1]));
        // Debug.Log("0 and 1 distance: " + (Vector3.Distance(points[0], points[1]) <= 0.7 * Mathf.Min(scaleValues)));

        while (Vector3.Distance(points[0], points[2]) <= 0.7 * Mathf.Min(scaleValues) || Vector3.Distance(points[1], points[2]) <= 0.7 * Mathf.Min(scaleValues))
            points[2] = GetRandomPointOnMesh(lookupCollider.sharedMesh) + lookupCollider.transform.position;
        
        // Debug.Log("0 and 2 distance: " + Vector3.Distance(points[0], points[2]));
        // Debug.Log("0 and 2 distance: " + (Vector3.Distance(points[0], points[2]) <= 0.7 * Mathf.Min(scaleValues)));
        // Debug.Log("1 and 2 distance: " + Vector3.Distance(points[1], points[2]));
        // Debug.Log("1 and 2 distance: " + (Vector3.Distance(points[1], points[2]) <= 0.7 *  Mathf.Min(scaleValues)));

        GameObject point0 = new GameObject("point0");
        point0.transform.position = points[0];
        point0.transform.parent = transform;
        childPoints.Add(point0);
        
        GameObject point1 = new GameObject("point1");
        point1.transform.position = points[1];
        point1.transform.parent = transform;
        childPoints.Add(point1);
        
        GameObject point2 = new GameObject("point2");
        point2.transform.position = points[2];
        point2.transform.parent = transform;
        childPoints.Add(point2);
    }
    
    private Vector3 GetRandomPointOnMesh(Mesh mesh)
    {
        //if you're repeatedly doing this on a single mesh, you'll likely want to cache cumulativeSizes and total
        float[] sizes = GetTriSizes(mesh.triangles, mesh.vertices);
        float[] cumulativeSizes = new float[sizes.Length];
        float total = 0;

        for (int i = 0; i < sizes.Length; i++)
        {
            total += sizes[i];
            cumulativeSizes[i] = total;
        }

        //so everything above this point wants to be factored out

        float randomsample = Random.value* total;

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

        //generate random barycentric coordinates
        float r = Random.value;
        float s = Random.value;

        if(r + s >=1)
        {
            r = 1 - r;
            s = 1 - s;
        }
        
        //and then turn them back to a Vector3
        Vector3 pointOnMesh = a + r*(b - a) + s*(c - a);
        pointOnMesh.Scale(transform.localScale);
        return pointOnMesh;
    }

    private float[] GetTriSizes(int[] tris, Vector3[] verts)
    {
        int triCount = tris.Length / 3;
        float[] sizes = new float[triCount];
        for (int i = 0; i < triCount; i++)
        {
            sizes[i] = .5f*Vector3.Cross(verts[tris[i*3 + 1]] - verts[tris[i*3]], verts[tris[i*3 + 2]] - verts[tris[i*3]]).magnitude;
        }
        return sizes;
    }
}
