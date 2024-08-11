using System;
using System.Collections.Generic;
using Miscellaneous.Utility;
using Scriptable_Object_Templates.Singletons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resource_Nodes.Asteroid
{
    public class Asteroid : ResourceNode
    {
        private const int MinOreCount = 1;
        private const int MaxOreCount = 5;
        
        private GameObject _orePrefab;
    
        public GameObject wholeAsteroid;
        [SerializeField] private GameObject fracturedAsteroid;
        
        private Rigidbody _wholeAsteroidRb;
        private Quaternion _asteroidRotationOffset;
        
        [SerializeField] private float explosionRadius = 1f;
        [SerializeField] private float explosionPower = 1f;

        [SerializeField] private Mesh asteroidMesh;
        [SerializeField] private GameObject asteroidPointPrefab;

        private const int MinPointsCount = 3;
        private const int MaxPointsCount = 6;
        
        private readonly List<AsteroidPoint> _asteroidPoints = new();

        public EventHandler<Asteroid> OnAsteroidDestroyed;
        
        private void Start()
        {
            _wholeAsteroidRb = wholeAsteroid.GetComponent<Rigidbody>();
            _asteroidRotationOffset = wholeAsteroid.transform.localRotation;
            
            _orePrefab = ResourcePrefabDictionary.Instance.dictionary[associatedResource.resourceType];
            
            foreach (var position in RandomPointSelector.GenerateRandomPointsOnMesh(asteroidMesh,
                         Random.Range(MinPointsCount, MaxPointsCount + 1), 0.1f))
            {
                var pointObject = Instantiate(asteroidPointPrefab, transform.TransformPoint(position), 
                    Quaternion.identity, wholeAsteroid.transform);
                var pointComponent = pointObject.GetComponent<AsteroidPoint>();

                pointComponent.OnPointDestroyed += HandleAsteroidPointDestroyed;
                _asteroidPoints.Add(pointComponent);
            }
        }

        private void HandleAsteroidPointDestroyed(object sender, AsteroidPoint point)
        {
            point.OnPointDestroyed -= HandleAsteroidPointDestroyed;

            _asteroidPoints.Remove(point);
            
            if (_asteroidPoints.Count <= 0)
            {
                ShatterAsteroid();
            }
        }

        private void ShatterAsteroid()
        {
            var asteroidPosition = wholeAsteroid.transform.position;
            
            // TODO: do we need both the normal and fractured? cant we just use the fractured on its own
            fracturedAsteroid.transform.position = asteroidPosition;
            fracturedAsteroid.transform.rotation = wholeAsteroid.transform.localRotation 
                                                   * Quaternion.Inverse(_asteroidRotationOffset);
            
            var explosionPos = fracturedAsteroid.transform.position;
            var asteroidVelocity = _wholeAsteroidRb.velocity;
        
            wholeAsteroid.SetActive(false);
            fracturedAsteroid.SetActive(true);
        
            // Spawn ore at center
            var oreNumber = Random.Range(MinOreCount, MaxOreCount + 1);
            
            while (oreNumber > 0)
            {
                Instantiate(_orePrefab, asteroidPosition, Quaternion.Euler(Random.Range(200, 360), 
                    Random.Range(200, 360), Random.Range(200, 360)));
                
                asteroidPosition[Random.Range(0, 3)] += Random.Range(0.4f, 0.9f);
                
                oreNumber--;
            }
        
            var colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        
            foreach (var overlappingCollider in colliders)
            {
                var rb = overlappingCollider.GetComponent<Rigidbody>();
        
                if (rb == null)
                    return;

                rb.velocity = asteroidVelocity;
                rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 0);
                
                // TODO: could this not accidentally unparent something apart from the asteroid?
                rb.transform.parent = null;
            }
        
            Destroy(wholeAsteroid);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            foreach (var asteroidPoint in _asteroidPoints)
            {
                asteroidPoint.OnPointDestroyed -= HandleAsteroidPointDestroyed;
            }
            
            OnAsteroidDestroyed?.Invoke(this, this);
        }
    }
}