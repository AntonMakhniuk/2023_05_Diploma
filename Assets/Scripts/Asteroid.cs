
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour, IDestructible
{
   [SerializeField] private double MaxHP { get;  set; } = 1000; // Example max HP
    private double CurrentHP { get;   set; }
  //  public IInstrument CurrentInstrument { get; set; }

    private int asteroidPointsCount = 0;
    [SerializeField] private GameObject spaceOre;
    [SerializeField] private bool shatter = false;

    private void Start()
    {
        CurrentHP = MaxHP;
        // Count the initial number of asteroid points
        asteroidPointsCount = 3;

        AsteroidPoint[] asteroidPoints = GetComponentsInChildren<AsteroidPoint>();
        foreach (var point in asteroidPoints)
        {
            point.SetUp(this);
        }
    }

    private void Update()
    {
        if (shatter)
            ShatterAsteroid();
    }

    double IDestructible.MaxHP
    {
        get => MaxHP;
        set => MaxHP = value;
    }

    double IDestructible.CurrentHP
    {
        get => MaxHP;
        set => MaxHP = value;
    }


    public void OnLaserInteraction(double damage)
    {
        Debug.Log("Asteroid can not take damage from laser");
    }

    public void OnDrillInteraction(double damage)
    {
        TakeDamage(damage);
    }


    public void OnCutterInteraction(double damage)
    {
        TakeDamage(damage);
    }

    public void OnExplosivesInteraction(double damage)
    {
        TakeDamage(damage);
    }

    public void TakeDamage(double damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            InitiateDestroy();
        }
    }

    public void InitiateDestroy()
    {
        ShatterAsteroid();
    }

    public void OnAsteroidPointDestroyed()
    {
        // Decrement the count of remaining asteroid points
        asteroidPointsCount--;

        // Take damage based on the number of asteroid points
        TakeDamage(MaxHP / 3);

        // Check if all points are destroyed
        if (asteroidPointsCount <= 0)
        {
            // Shatter the entire asteroid into random pieces
            ShatterAsteroid();
        }
    }

    public void ShatterAsteroid()
    {
        // Dynamically create a plane using asteroid's geometry
        Plane slicingPlane = CreateSlicingPlane();

        // Slice the asteroid with the dynamically created plane
        GameObject[] slices = Slicer.Slice(slicingPlane, gameObject);

        // Optionally, you can add forces or other effects to the sliced pieces
        Vector3 pushVector = new Vector3(0, 0, 0);
        pushVector[Random.Range(0, 3)] = Random.Range(250f, 450f);
        pushVector[Random.Range(0, 3)] = Random.Range(250f, 320f);
        foreach (GameObject slice in slices)
        {
            pushVector *= -1;
            slice.GetComponent<Rigidbody>().AddRelativeForce(pushVector);
        }

        // Spawn ore at center 
        Vector3 asteroidPosition = transform.position;
        int oreNumber = Random.Range(1, 6);
        while (oreNumber > 0)
        {
            SpawnOre(asteroidPosition);
            asteroidPosition[Random.Range(0, 3)] += Random.Range(0.51f, 0.7f);
            oreNumber--;
        }

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

    private void SpawnOre(Vector3 position)
    {
        Instantiate(spaceOre, position, Quaternion.Euler(Random.Range(200, 360), Random.Range(200, 360), Random.Range(200, 360)));
    }

}
