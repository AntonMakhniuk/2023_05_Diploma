using System;
using System.Collections.Generic;
using System.Linq;
using Production.Challenges.General;
using Production.Crafting;
using UnityEngine;

namespace Production.Systems
{
    // meant to be created every time a new production is started, and destroyed afterwards
    public class ProductionSessionManager : MonoBehaviour
    {
        // TODO: add modifier changes to the CraftingData object
        
        // prefabs that the manager may use in the production session
        private GameObject[] _generalChallengePrefabs;
        private GameObject[] _resourceChallengePrefabs;
        
        public Difficulty Difficulty { get; private set; }
        public CraftingData CraftingData;
        
        private readonly int _maxCriticalFails = 3;
        private int _criticalFailCount;
        
        // currently running instances of session-specific challenges
        private readonly List<GameObject> _generalChallengeInstances = new();
        private List<GameObject> _resourceChallengeInstances = new();

        [SerializeField] private GameObject[] extraPrefabsForInstantiation;
        
        public void Setup(CraftingData craftingData,
            GameObject[] permittedGeneralChallenges, GameObject[] permittedResourceChallenges)
        {
            foreach (var extraPrefab in extraPrefabsForInstantiation)
            {
                Instantiate(extraPrefab, transform);
            }
            
            CraftingData = craftingData;
            Difficulty = craftingData.Recipe.difficulty;
            _generalChallengePrefabs = permittedGeneralChallenges;
            _resourceChallengePrefabs = permittedResourceChallenges;
            
            // TODO: implement logic for choosing difficulty randomly
            
            foreach (GameObject generalPrefab in _generalChallengePrefabs)
            {
                var instance = Instantiate(generalPrefab, transform);
                _generalChallengeInstances.Add(instance);
            }
            
            // TODO: introduce algorithm that spawns challenges over time
            
            foreach (IGeneralChallenge generalInstance in _generalChallengeInstances
                         .Select(ch => ch.GetComponent<IGeneralChallenge>())
                         .ToList())
            {
                generalInstance.SubscribeToOnGeneralFail(AddCriticalFail);
            }
        }

        public event EventHandler<int> OnCriticalFailReachedManager;
        
        private void AddCriticalFail()
        {
            _criticalFailCount++;
            
            OnCriticalFailReachedManager?.Invoke(this, _criticalFailCount);
            
            if (_criticalFailCount >= _maxCriticalFails)
            {
                FailProduction();
            }
        }

        public event EventHandler OnProductionFailed;
        
        private void FailProduction()
        {
            foreach (IGeneralChallenge generalInstance in _generalChallengeInstances
                         .Select(ch => ch.GetComponent<IGeneralChallenge>())
                         .ToList())
            {
                generalInstance.UnsubscribeFromOnGeneralFail(AddCriticalFail);
            }
            
            foreach (var challengeInstance in _generalChallengeInstances)
            {
                Destroy(challengeInstance);
            }

            foreach (var challengeInstance in _resourceChallengeInstances)
            {
                Destroy(challengeInstance);
            }
            
            // TODO: Send out some kind of struct holding session data in the event arguments?
            
            OnProductionFailed?.Invoke(this, null);
        }
    }
}