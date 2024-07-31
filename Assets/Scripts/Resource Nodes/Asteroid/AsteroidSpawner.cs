using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resource_Nodes.Asteroid
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject asteroidPrefab;
        
        [SerializeField] private int startingNumberOfAsteroids = 10;
        [SerializeField] private int maxNumberOfAsteroids = 100;

        private List<Asteroid> _currentAsteroids = new();
        
        [SerializeField] private Vector3 spawnZoneSize = new Vector3(10f, 10f, 10f);
        [SerializeField] private float spawnInterval = 5f;
    
        [SerializeField] private SpawnShape spawnShape = SpawnShape.Rectangle;
        [SerializeField] private float circleRadius = 5f;
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private bool isRoom;

        [Header("Asteroid Movement Properties")]
        [SerializeField] private float asteroidSpeed = 1f;
        [SerializeField] private float asteroidRotationSpeed = 5f;

        private void Start()
        {
            SpawnStartingAsteroids();
            InvokeRepeating(nameof(SpawnSingleAsteroid), spawnInterval, spawnInterval);
        }

        private void SpawnStartingAsteroids()
        {
            for (int i = 0; i < startingNumberOfAsteroids; i++)
            {
                SpawnSingleAsteroid();
            }
        }

        private void SpawnSingleAsteroid()
        {
            if (_currentAsteroids.Count >= maxNumberOfAsteroids)
            {
                return;
            }
            
            var randomPosition = GetRandomPosition();
            var randomRotation = Quaternion.Euler(Random.Range(0, 360), 
                Random.Range(0, 360), Random.Range(0, 360));

            var asteroidObject = Instantiate(asteroidPrefab, randomPosition, randomRotation, transform);

            var asteroidComponent = asteroidObject.GetComponent<Asteroid>();
            asteroidComponent.OnAsteroidDestroyed += HandleAsteroidDestroyed;
            _currentAsteroids.Add(asteroidComponent);
            
            var asteroidRb = asteroidObject.GetComponent<Rigidbody>();
        
            asteroidRb.AddForce(Random.onUnitSphere * asteroidSpeed, ForceMode.Impulse);
            asteroidRb.AddTorque(Random.onUnitSphere * asteroidRotationSpeed, ForceMode.Impulse);
            
            if (isRoom)
            {
                StartCoroutine(ConstrainPosition(asteroidRb, spawnZoneSize));
            }
        }

        private void HandleAsteroidDestroyed(object sender, Asteroid asteroid)
        {
            _currentAsteroids.Remove(asteroid);
        }
        
        //TODO: very expensive to run on hundreds of asteroids in a while true loop, rework as OnTriggerExit?
        private IEnumerator ConstrainPosition(Rigidbody asteroidRb, Vector3 zoneSize)
        {
            while (true)
            {
                var position = transform.position;
                
                asteroidRb.position = new Vector3(
                    Mathf.Clamp(asteroidRb.position.x, 
                        position.x - zoneSize.x / 2, position.x + zoneSize.x / 2),
                    
                    Mathf.Clamp(asteroidRb.position.y, 
                        position.y - zoneSize.y / 2, position.y + zoneSize.y / 2),
                    
                    Mathf.Clamp(asteroidRb.position.z, 
                        position.z - zoneSize.z / 2, position.z + zoneSize.z / 2)
                );

                yield return null;
            }
        }

        private Vector3 GetRandomPosition()
        {
            var randomPosition = Vector3.zero;

            switch (spawnShape)
            {
                case SpawnShape.Rectangle:
                {
                    randomPosition = transform.position + new Vector3(
                        Random.Range(-spawnZoneSize.x / 2, spawnZoneSize.x / 2),
                        Random.Range(-spawnZoneSize.y / 2, spawnZoneSize.y / 2),
                        Random.Range(-spawnZoneSize.z / 2, spawnZoneSize.z / 2)
                    );

                    break;
                }
                case SpawnShape.Circle:
                {
                    var angle = Random.Range(0f, 2f * Mathf.PI);
                    
                    randomPosition = transform.position + new Vector3(
                        Mathf.Cos(angle) * circleRadius,
                        Random.Range(-spawnZoneSize.y / 2, spawnZoneSize.y / 2),
                        Mathf.Sin(angle) * circleRadius
                    );
                    
                    break;
                }

                case SpawnShape.Sphere:
                {
                    randomPosition = transform.position + Random.onUnitSphere * circleRadius;
                    
                    break;
                }
            }

            return randomPosition;
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmos)
            {
                return;
            }

            Gizmos.color = Color.blue;

            switch (spawnShape)
            {
                case SpawnShape.Rectangle:
                {
                    Gizmos.DrawWireCube(transform.position, spawnZoneSize);
                    
                    break;
                }
                case SpawnShape.Sphere:
                case SpawnShape.Circle:
                {
                    Gizmos.DrawWireSphere(transform.position, circleRadius);
                    
                    break;
                }
            }
        }

        private enum SpawnShape
        {
            Rectangle, Circle, Sphere
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
