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
        
        public Difficulty difficulty;
        
        private readonly int _maxCriticalFails = 3;
        private int _criticalFailCount;
        
        // currently running instances of session-specific challenges
        private List<GameObject> _generalChallengeInstances;
        private List<GameObject> _resourceChallengeInstances;
        
        private void Start()
        {
            // TODO: add logic for fetching challenges from the registry
            // TODO: implement logic for choosing difficulty randomly
            
            foreach (GameObject generalPrefab in _generalChallengePrefabs)
            {
                Instantiate(generalPrefab);
                _generalChallengeInstances.Add(generalPrefab);
            }
            
            foreach (GameObject resourcePrefab in _resourceChallengePrefabs)
            {
                Instantiate(resourcePrefab);
                _resourceChallengeInstances.Add(resourcePrefab);
            }
            
            foreach (IGeneralChallenge generalInstance in _generalChallengeInstances
                         .Select(chal => chal.GetComponent<IGeneralChallenge>())
                         .ToList())
            {
                generalInstance.SubscribeToOnGeneralFail(AddCriticalFail);
            }
        }

        public void OnDestroy()
        {
            foreach (IGeneralChallenge generalInstance in _generalChallengeInstances
                         .Select(chal => chal.GetComponent<IGeneralChallenge>())
                         .ToList())
            {
                generalInstance.UnsubscribeFromOnGeneralFail(AddCriticalFail);
            }

            foreach (GameObject generalInstance in _generalChallengeInstances)
            {
                Destroy(generalInstance);
            }
            
            foreach (GameObject resourceInstance in _resourceChallengeInstances)
            {
                Destroy(resourceInstance);
            }
        }

        private void AddCriticalFail()
        {
            if (_criticalFailCount < _maxCriticalFails)
            {
                _criticalFailCount++;
            }
            else
            {
                FailProduction();
            }
        }

        private void FailProduction()
        {
            throw new NotImplementedException();
        }
    }
}