using System.Collections.Generic;
using Miscellaneous.Utility;
using Systems.Mining.Resource_Nodes.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Systems.Mining.Resource_Nodes.Asteroid
{
    public class ResourceAsteroid : Asteroid
    {
        [Header("Child Point Data")]
        [SerializeField] private GameObject asteroidPointPrefab;
        [SerializeField] private float minDistanceBetweenPoints = 0.1f;
        [SerializeField] private int minPointsCount = 3;
        [SerializeField] private int maxPointsCount = 6;
        
        private readonly List<ResourceNode> _asteroidPoints = new();

        protected override void Start()
        {
            var mesh = GetComponent<MeshFilter>().mesh;
            
            foreach (var (position, rotation) in RandomPointSelector.GenerateRandomPointsOnMesh(
                         mesh,
                         Random.Range(minPointsCount, maxPointsCount + 1), 
                         minDistanceBetweenPoints))
            {
                var pointObject = Instantiate(asteroidPointPrefab, transform.TransformPoint(position), 
                    rotation, transform);
                var pointComponent = pointObject.GetComponentInChildren<AsteroidPoint>();
                
                pointComponent.destroyed.AddListener(HandleAsteroidPointDestroyed);
                _asteroidPoints.Add(pointComponent);
            }

            base.Start();
        }

        private void HandleAsteroidPointDestroyed(ResourceNode point)
        {
            point.destroyed.RemoveListener(HandleAsteroidPointDestroyed);

            _asteroidPoints.Remove(point);
            
            point.transform.parent.parent = transform.parent;
            
            if (_asteroidPoints.Count <= 0)
            {
                InitiateDestroy();
            }
        }

        public override void InitiateDestroy()
        {
            foreach (var asteroidPoint in _asteroidPoints)
            {
                asteroidPoint.destroyed.RemoveListener(HandleAsteroidPointDestroyed);
                asteroidPoint.transform.parent.parent = transform.parent;
                asteroidPoint.InitiateDestroy();
            }
            
            base.InitiateDestroy();
        }

        protected override void OnLaserInteraction()
        {
            // No special behavior
        }

        protected override void OnBombInteraction()
        {
            // No special behavior
        }
    }
}