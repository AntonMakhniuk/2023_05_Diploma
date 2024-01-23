using System;
using System.Collections.Generic;
using System.Linq;
using Production.Challenges.General;
using UnityEngine;

namespace Production
{
    // meant to be created every time a new production is started, and destroyed afterwards
    public class ProductionSessionManager : MonoBehaviour
    {
        // prefabs that the manager may use in the production session
        private GameObject[] _generalChallengePrefabs;
        private GameObject[] _resourceChallengePrefabs;
        
        public Difficulty Difficulty { get; private set; }
        
        private readonly int _maxCriticalFails = 3;
        private int _criticalFailCount;
        
        // currently running instances of session-specific challenges
        private List<GameObject> _generalChallengeInstances = new();
        private List<GameObject> _resourceChallengeInstances = new();

        [SerializeField] private GameObject[] extraPrefabsForInstantiation;
        
        public void Setup(Difficulty givenDifficulty, 
            GameObject[] permittedGeneralChallenges, GameObject[] permittedResourceChallenges)
        {
            foreach (var extraPrefab in extraPrefabsForInstantiation)
            {
                Instantiate(extraPrefab, transform);
            }
            
            Difficulty = givenDifficulty;
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

        public void OnDestroy()
        {
            foreach (IGeneralChallenge generalInstance in _generalChallengeInstances
                         .Select(ch => ch.GetComponent<IGeneralChallenge>())
                         .ToList())
            {
                generalInstance.UnsubscribeFromOnGeneralFail(AddCriticalFail);
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
            gameObject.SetActive(false);
            
            OnProductionFailed?.Invoke(this, null);
        }
    }
}