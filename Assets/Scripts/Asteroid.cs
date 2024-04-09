using System;
using System.Collections;
using Assets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour 
{
    private int asteroidPointsCount = 0;
    [SerializeField] private GameObject spaceOre;
    [SerializeField] private bool shatter = false;

    private void Start()
    {
        // Count the initial number of asteroid points
        asteroidPointsCount = 3;
    }

    private void Update() {
        if (shatter)
            ShatterAsteroid();
    }

    public void OnAsteroidPointDestroyed()
    {
        // Decrement the count of remaining asteroid points
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
        Vector3 pushVector = new Vector3(0,0,0);
        pushVector[Random.Range(0, 3)] = Random.Range(250f, 450f);
        pushVector[Random.Range(0, 3)] = Random.Range(250f, 320f);
        foreach (GameObject slice in slices) {
            pushVector *= -1;
            slice.GetComponent<Rigidbody>().AddRelativeForce(pushVector);
        }
        
        // Spawn ore at center 
        Vector3 asteroidPosition = transform.position;
        int oreNumber = Random.Range(1, 6);
        while (oreNumber > 0) {
            SpawnOre(asteroidPosition);
            asteroidPosition[Random.Range(0,3)] += Random.Range(0.51f,0.7f);
            oreNumber--;
        }
        
        // Destroy the original asteroid GameObject
        Destroy(gameObject);
    }
    public void Explode()
    {
        // Dynamically create a slicing plane for explosion
        Plane explosionPlane = CreateExplosionPlane();

        // Slice the asteroid with the explosion slicing plane
        GameObject[] slices = Slicer.Slice(explosionPlane, gameObject);

        // Optionally, you can add forces or other effects to the sliced pieces
        Vector3 pushVector = new Vector3(0, 0, 0);
        pushVector[Random.Range(0, 3)] = Random.Range(250f, 450f);
        pushVector[Random.Range(0, 3)] = Random.Range(250f, 320f);
        foreach (GameObject slice in slices)
        {
            pushVector *= -1;
            slice.GetComponent<Rigidbody>().AddRelativeForce(pushVector);
        }

        // Spawn ore at each slice's position
        foreach (GameObject slice in slices)
        {
            Vector3 slicePosition = slice.transform.position;
            int oreNumber = Random.Range(1, 4); // Spawn fewer ores per slice
            oreNumber = Mathf.Max(1, oreNumber / 3); // Reduce ore amount by 3 times
            while (oreNumber > 0)
            {
                SpawnOre(slicePosition);
                slicePosition += Random.insideUnitSphere * 0.2f; // Randomize ore spawn position around the slice
                oreNumber--;
            }
        }

        // Destroy the original asteroid GameObject
        Destroy(gameObject);
    }
    
    private Plane CreateExplosionPlane()
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

        // Randomize the slicing direction
        Vector3 normal = Random.insideUnitSphere.normalized;

        // Create and return the plane
        return new Plane(normal, center);
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

    private void SpawnOre(Vector3 position) {
        Instantiate(spaceOre, position, Quaternion.Euler(Random.Range(200,360), Random.Range(200,360), Random.Range(200,360)));
    }
}