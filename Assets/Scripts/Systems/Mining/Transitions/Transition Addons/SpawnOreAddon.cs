using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Systems.Mining.Transitions.Transition_Addons
{
    public class SpawnOreAddon : BaseTransitionAddon
    {
        [Header("Ore Data")] 
        [SerializeField] private GameObject orePrefab;
        [SerializeField] private int minOreCount = 1;
        [SerializeField] private int maxOreCount = 5;
        [SerializeField] private float minSpawnDistanceOffset = 0.4f;
        [SerializeField] private float maxSpawnDistanceOffset = 0.9f;
        [Space]
        [Header("Miscellaneous")]
        [SerializeField] private float spawnDelay = 0.5f;
        [SerializeField] private int maxRetriesOnOverlap = 10;
        
        public override void ApplyEffect()
        {
            StartCoroutine(SpawnOreCoroutine());
        }

        private IEnumerator SpawnOreCoroutine()
        {
            yield return new WaitForSeconds(spawnDelay);
            
            var oreNumber = Random.Range(minOreCount, maxOreCount + 1);
            
            while (oreNumber > 0)
            {
                Vector3 orePos;
                bool isOverlapping;
                var retries = 0;

                do
                {
                    orePos = transform.position;

                    for (var i = 0; i < 3; i++)
                    {
                        orePos[i] += Random.Range(minSpawnDistanceOffset, maxSpawnDistanceOffset) 
                                     * (Random.value < 0.5f ? -1 : 1);;
                    }

                    isOverlapping = Physics.CheckSphere(orePos, minSpawnDistanceOffset);
                    retries++;
                    
                    Debug.Log("spawn ore " + this);
                    
                } while (isOverlapping && retries < maxRetriesOnOverlap);

                if (!isOverlapping)
                {
                    var ore = Instantiate(orePrefab, orePos, Quaternion.Euler(Random.Range(0, 360),
                        Random.Range(0, 360), Random.Range(0, 360)));
                    oreNumber--;
                }
                else
                {
                    Debug.LogWarning("Could not find a non-overlapping position for " +
                                     "ore after multiple retries.");
                    
                    oreNumber--;
                }
            }
            
            base.ApplyEffect();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}