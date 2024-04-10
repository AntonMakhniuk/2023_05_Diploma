using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    [SerializeField]private int numberOfAsteroids = 10;
    [SerializeField]private Vector3 spawnZoneSize = new Vector3(10f, 10f, 10f);
    [SerializeField]private float spawnInterval = 5f;

    public enum SpawnShape { Rectangle, Circle, Sphere }
    [SerializeField]private SpawnShape spawnShape = SpawnShape.Rectangle;
    [SerializeField]private float circleRadius = 5f;
    [SerializeField]private bool drawGizmos = true;
    [SerializeField] private bool isRoom = false;

    [Header("Asteroid Properties")]
    [SerializeField]private float asteroidSpeed = 1f;
    [SerializeField]private float asteroidRotationSpeed = 5f;

    private void Start()
    {
        StartCoroutine(SpawnAsteroidsWithDelay());
    }

    private IEnumerator SpawnAsteroidsWithDelay()
    {
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            SpawnSingleAsteroid();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnSingleAsteroid()
    {
        Vector3 randomPosition = GetRandomPosition();
        Quaternion randomRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        GameObject asteroid = Instantiate(asteroidPrefab, randomPosition, randomRotation);
        Rigidbody asteroidRb = asteroid.GetComponent<Rigidbody>();
        asteroidRb.AddForce(Random.onUnitSphere * asteroidSpeed, ForceMode.Impulse);
        asteroidRb.AddTorque(Random.onUnitSphere * asteroidRotationSpeed, ForceMode.Impulse);

        asteroid.transform.parent = transform;
        
        if (isRoom)
        {
            StartCoroutine(ConstrainPosition(asteroid, spawnZoneSize));
        }
    }
    
    private IEnumerator ConstrainPosition(GameObject obj, Vector3 zoneSize)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        while (true)
        {
            rb.position = new Vector3(
                Mathf.Clamp(rb.position.x, transform.position.x - zoneSize.x / 2, transform.position.x + zoneSize.x / 2),
                Mathf.Clamp(rb.position.y, transform.position.y - zoneSize.y / 2, transform.position.y + zoneSize.y / 2),
                Mathf.Clamp(rb.position.z, transform.position.z - zoneSize.z / 2, transform.position.z + zoneSize.z / 2)
            );

            yield return null;
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = Vector3.zero;

        switch (spawnShape)
        {
            case SpawnShape.Rectangle:
                randomPosition = transform.position + new Vector3(
                    Random.Range(-spawnZoneSize.x / 2, spawnZoneSize.x / 2),
                    Random.Range(-spawnZoneSize.y / 2, spawnZoneSize.y / 2),
                    Random.Range(-spawnZoneSize.z / 2, spawnZoneSize.z / 2)
                );
                break;

            case SpawnShape.Circle:
                float angle = Random.Range(0f, 2f * Mathf.PI);
                randomPosition = transform.position + new Vector3(
                    Mathf.Cos(angle) * circleRadius,
                    Random.Range(-spawnZoneSize.y / 2, spawnZoneSize.y / 2),
                    Mathf.Sin(angle) * circleRadius
                );
                break;

            case SpawnShape.Sphere:
                randomPosition = transform.position + Random.onUnitSphere * circleRadius;
                break;
        }

        return randomPosition;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        Gizmos.color = Color.blue;

        switch (spawnShape)
        {
            case SpawnShape.Rectangle:
                Gizmos.DrawWireCube(transform.position, spawnZoneSize);
                break;

            case SpawnShape.Circle:
                Gizmos.DrawWireSphere(transform.position, circleRadius);
                break;

            case SpawnShape.Sphere:
                Gizmos.DrawWireSphere(transform.position, circleRadius);
                break;
        }
    }
}
