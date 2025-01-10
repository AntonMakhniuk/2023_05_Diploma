using System.Collections;
using NaughtyAttributes;
using Systems.Mining.Resource_Nodes.Base;
using Systems.Mining.Transitions.Transition_Addons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Systems.Mining.Addons
{
    public class SpawnObjectAddon : BaseTransitionAddon
    {
        [Foldout("Spawned Object Data")] [SerializeField] 
        private GameObject objectPrefab;
        [Foldout("Spawned Object Data")] [SerializeField] 
        private int minCount = 1;
        [Foldout("Spawned Object Data")] [SerializeField] 
        private int maxCount = 5;
        [Foldout("Spawned Object Data")] [SerializeField] 
        private float minSpawnDistanceOffset = 0.4f;
        [Foldout("Spawned Object Data")] [SerializeField] 
        private float maxSpawnDistanceOffset = 0.9f;
        [Foldout("Spawned Object Data")] [MinMaxSlider(0.5f, 2f)] [SerializeField] 
        private Vector2 objectScaleInRelationToNode = new(0.9f, 1.1f);
        [Foldout("Spawned Object Data")] [SerializeField]
        private float minScale = 0.5f;
        
        [Foldout("Miscellaneous")]
        [SerializeField] private ResourceNode nodeToSpawnAt;
        [Foldout("Miscellaneous")]
        [SerializeField] private float spawnDelay = 0.5f;

        private const int MaxScaleTries = 100;
        
        private Vector3 _baseSpawnPosition;
        private float _nodeScale;
        
        private void Start()
        {
            nodeToSpawnAt.destroyed.AddListener(UpdateNodeData);
        }

        private void UpdateNodeData(ResourceNode node)
        {
            var nodeTransform = node.transform;
            var scaleVector = nodeTransform.lossyScale;
            
            _baseSpawnPosition = nodeTransform.position;
            _nodeScale = (scaleVector.x + scaleVector.y + scaleVector.z) / 3f;
        }

        public override void ApplyEffect()
        {
            StartCoroutine(SpawnOreCoroutine());
        }

        private IEnumerator SpawnOreCoroutine()
        {
            yield return new WaitForSeconds(spawnDelay);
            
            var oreNumber = Random.Range(minCount, maxCount + 1);
            
            while (oreNumber > 0)
            {
                var spawnPos = _baseSpawnPosition;
                    
                for (var i = 0; i < 3; i++)
                {
                    spawnPos[i] += Random.Range(minSpawnDistanceOffset, maxSpawnDistanceOffset * _nodeScale) 
                                   * (Random.value < 0.5f ? -1 : 1);
                }
                
                var ore = Instantiate(objectPrefab, spawnPos, 
                    Quaternion.Euler(Random.Range(0, 360),
                        Random.Range(0, 360), Random.Range(0, 360)));

                var tryCount = 0;
                
                do
                {
                    ore.transform.localScale 
                        *= _nodeScale * Random.Range(objectScaleInRelationToNode.x, objectScaleInRelationToNode.y);

                    tryCount++;
                } 
                while (ore.transform.localScale.x < minScale && tryCount < MaxScaleTries);
                
                oreNumber--;
            }
            
            base.ApplyEffect();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            
            nodeToSpawnAt.destroyed.RemoveListener(UpdateNodeData);
        }
    }
}