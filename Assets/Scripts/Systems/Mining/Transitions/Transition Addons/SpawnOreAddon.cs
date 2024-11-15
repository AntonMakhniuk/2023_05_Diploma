using UnityEngine;

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
        
        public override void ApplyEffect()
        {
            var oreNumber = Random.Range(minOreCount, maxOreCount + 1);
            
            while (oreNumber > 0)
            {
                var orePos = transform.position;
                orePos[Random.Range(0, 3)] += Random.Range(minSpawnDistanceOffset, maxSpawnDistanceOffset);
                
                Instantiate(orePrefab, orePos, Quaternion.Euler(Random.Range(0, 360), 
                    Random.Range(0, 360), Random.Range(0, 360)));
                
                oreNumber--;
            }
            
            base.ApplyEffect();
        }
    }
}