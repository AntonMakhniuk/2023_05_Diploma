using Assets.Scripts;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private int asteroidPointsCount = 0;

    private void Start()
    {
        // Count the initial number of asteroid points
        asteroidPointsCount = 3;
        Debug.Log(asteroidPointsCount);
    }

    public void OnAsteroidPointDestroyed()
    {
        // Decrement the count of remaining asteroid points
        Debug.Log(asteroidPointsCount);
        asteroidPointsCount--;

        // Check if all points are destroyed
        if (asteroidPointsCount <= 0)
        {
            // Shatter the entire asteroid into random pieces
            ShatterAsteroid();
        }
    }

    void ShatterAsteroid()
    {
        // Dynamically create a plane using asteroid's geometry
        Plane slicingPlane = CreateSlicingPlane();

        // Slice the asteroid with the dynamically created plane
        GameObject[] slices = Slicer.Slice(slicingPlane, gameObject);

        // Optionally, you can add forces or other effects to the sliced pieces

        // Destroy the original asteroid GameObject
        Destroy(gameObject);
    }

    private Plane CreateSlicingPlane()
    {
        // Get the mesh from the asteroid
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        // Compute the average position of vertices to get the center
        Vector3 center = Vector3.zero;
        foreach (Vector3 vertex in mesh.vertices)
        {
            center += vertex;
        }
        center /= mesh.vertices.Length;

        // Use the normal of the mesh as the normal of the slicing plane
        Vector3 normal = transform.TransformDirection(mesh.normals[0]);

        // Create and return the plane
        return new Plane(normal, center);
    }
}
