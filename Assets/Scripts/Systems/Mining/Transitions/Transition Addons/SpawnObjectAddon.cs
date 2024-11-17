using System.Collections;
using Systems.Mining.Resource_Nodes.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Systems.Mining.Transitions.Transition_Addons
{
    public class SpawnObjectAddon : BaseTransitionAddon
    {
        [Header("Ore Data")] 
        [SerializeField] private GameObject objectPrefab;
        [SerializeField] private int minCount = 1;
        [SerializeField] private int maxCount = 5;
        [SerializeField] private float minSpawnDistanceOffset = 0.4f;
        [SerializeField] private float maxSpawnDistanceOffset = 0.9f;
        [Space]
        [Header("Miscellaneous")]
        [SerializeField] private ResourceNode nodeToSpawnAt;
        [SerializeField] private float spawnDelay = 0.5f;
        [SerializeField] private int maxRetriesOnOverlap = 10;

        private Vector3 _baseSpawnPosition;
        
        private void Start()
        {
            nodeToSpawnAt.destroyed.AddListener(UpdateSpawnLocation);
        }

        private void UpdateSpawnLocation(ResourceNode node)
        {
            _baseSpawnPosition = node.transform.position;
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
                Vector3 spawnPos;
                bool isOverlapping;
                var retries = 0;
                
                do
                {
                    spawnPos = _baseSpawnPosition;
                    
                    for (var i = 0; i < 3; i++)
                    {
                        spawnPos[i] += Random.Range(minSpawnDistanceOffset, maxSpawnDistanceOffset) 
                                     * (Random.value < 0.5f ? -1 : 1);;
                    }

                    isOverlapping = Physics.CheckSphere(spawnPos, minSpawnDistanceOffset);
                    retries++;
                    
                    
                } while (isOverlapping && retries < maxRetriesOnOverlap);

                if (!isOverlapping)
                {
                    var ore = Instantiate(objectPrefab, spawnPos, Quaternion.Euler(Random.Range(0, 360),
                        Random.Range(0, 360), Random.Range(0, 360)));
                    oreNumber--;
                }
                else
                {
                    Debug.LogWarning("Could not find a valid spawn position for ore.");
                    
                    oreNumber--;
                }
            }
            
            base.ApplyEffect();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            
            nodeToSpawnAt.destroyed.RemoveListener(UpdateSpawnLocation);
        }
    }
}