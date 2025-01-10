using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Scriptable_Object_Templates.Singletons;
using Systems.Mining.Resource_Nodes.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Systems.Mining.Resource_Nodes.Asteroid
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [Foldout("Spawn Zone Properties")]
        [SerializeField] private Vector3 spawnZoneSize = new(10f, 10f, 10f);
        [Foldout("Spawn Zone Properties")]
        [SerializeField] private float spawnInterval = 5f;
        [Foldout("Spawn Zone Properties")]
        [SerializeField] private SpawnShape spawnShape = SpawnShape.Rectangle;
        [Foldout("Spawn Zone Properties")] [ShowIf("spawnShape", SpawnShape.Circle)] 
        [SerializeField] private float circleRadius = 5f;
        [Foldout("Spawn Zone Properties")]
        [SerializeField] private bool drawGizmos = true;
        [Foldout("Spawn Zone Properties")]
        [SerializeField] private bool isRoom;

        [Foldout("Asteroid Properties")] [Range(0f, 1f)]
        [SerializeField] private float fillerRatio = 0.7f;
        [Foldout("Asteroid Properties")]
        [SerializeField] private int startingNumberOfAsteroids = 10;
        [Foldout("Asteroid Properties")]
        [SerializeField] private int maxNumberOfAsteroids = 100;
        [Foldout("Asteroid Properties")]
        [SerializeField] private float asteroidSpeed = 1f;
        [Foldout("Asteroid Properties")]
        [SerializeField] private float asteroidRotationSpeed = 5f;
        [Foldout("Asteroid Properties")] [MinMaxSlider(1f, 20f)]
        [SerializeField] private Vector2 minMaxAsteroidScale = new Vector2(3f, 7f);

        private readonly List<ResourceNode> _currentAsteroids = new();
        
        private void Start()
        {
            SpawnStartingAsteroids();
            InvokeRepeating(nameof(SpawnSingleAsteroid), spawnInterval, spawnInterval);
        }

        private void SpawnStartingAsteroids()
        {
            for (var i = 0; i < startingNumberOfAsteroids; i++)
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
            
            var asteroidType = Random.value > fillerRatio
                ? ResourceNodeType.ResourceAsteroid
                : ResourceNodeType.FillerAsteroid;

            var asteroidObject = asteroidType == ResourceNodeType.ResourceAsteroid
                ? Instantiate(ResourceNodePrefabDictionary.Instance
                        .GetRandomPrefabByType(ResourceNodeType.ResourceAsteroid), randomPosition, randomRotation,
                    transform)
                : Instantiate(ResourceNodePrefabDictionary.Instance
                        .GetRandomPrefabByType(ResourceNodeType.FillerAsteroid), randomPosition, randomRotation,
                    transform);

            asteroidObject.transform.localScale *= Random.Range(minMaxAsteroidScale.x, minMaxAsteroidScale.y);
            
            var asteroidComponent = asteroidObject.GetComponentInChildren<Asteroid>();
            
            asteroidComponent.destroyed.AddListener(HandleAsteroidDestroyed);
            _currentAsteroids.Add(asteroidComponent);
            
            var asteroidRb = asteroidComponent.GetComponent<Rigidbody>();
        
            asteroidRb.AddForce(Random.onUnitSphere * asteroidSpeed, ForceMode.Impulse);
            asteroidRb.AddTorque(Random.onUnitSphere * asteroidRotationSpeed, ForceMode.Impulse);
            
            if (isRoom)
            {
                StartCoroutine(ConstrainPosition(asteroidRb, spawnZoneSize));
            }
        }

        private void HandleAsteroidDestroyed(ResourceNode asteroid)
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
