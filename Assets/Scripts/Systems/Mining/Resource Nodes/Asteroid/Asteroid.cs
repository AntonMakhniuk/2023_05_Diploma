using System.Collections.Generic;
using Miscellaneous.Utility;
using Systems.Mining.Resource_Nodes.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Systems.Mining.Resource_Nodes.Asteroid
{
    public class Asteroid : ResourceNodeWithHealth
    {
        [Header("Asteroid Data")]
        public GameObject wholeAsteroid;
        [Space]
        [Header("Child Point Data")]
        [SerializeField] private GameObject asteroidPointPrefab;
        [SerializeField] private float minDistanceBetweenPoints = 0.1f;
        [SerializeField] private int minPointsCount = 3;
        [SerializeField] private int maxPointsCount = 6;
        
        private readonly List<ResourceNode> _asteroidPoints = new();

        protected override void Start()
        {
            var mesh = wholeAsteroid.GetComponent<MeshFilter>().mesh;
            
            foreach (var position in RandomPointSelector.GenerateRandomPointsOnMesh(
                         mesh,
                         Random.Range(minPointsCount, maxPointsCount + 1), 
                         minDistanceBetweenPoints))
            {
                var pointObject = Instantiate(asteroidPointPrefab, transform.TransformPoint(position), 
                    Quaternion.identity, wholeAsteroid.transform);
                var pointComponent = pointObject.GetComponent<AsteroidPoint>();

                pointComponent.destroyed.AddListener(HandleAsteroidPointDestroyed);
                _asteroidPoints.Add(pointComponent);
            }

            base.Start();
        }

        private void HandleAsteroidPointDestroyed(ResourceNode point)
        {
            point.destroyed.RemoveListener(HandleAsteroidPointDestroyed);

            _asteroidPoints.Remove(point);
            
            if (_asteroidPoints.Count <= 0)
            {
                Destroy(gameObject);
            }
        }

        protected override void OnLaserInteraction()
        {
            // No special behavior
        }

        protected override void OnBombInteraction()
        {
            // No special behavior
        }

        protected override void OnDestroy()
        {
            foreach (var asteroidPoint in _asteroidPoints)
            {
                asteroidPoint.destroyed.RemoveListener(HandleAsteroidPointDestroyed);
                
                Destroy(asteroidPoint);
            }
            
            base.OnDestroy();
        }
    }
}