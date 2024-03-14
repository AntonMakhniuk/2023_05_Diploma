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
        
        [HideInInspector] public int criticalFailCount;
        
        public Difficulty Difficulty { get; private set; }
        public CraftingData CraftingData;
        public int maxCriticalFails = 3;

        // currently running instances of session-specific challenges
        private readonly List<GameObject> _generalChallengeInstances = new();
        private List<GameObject> _resourceChallengeInstances = new();

        [SerializeField] private GameObject[] extraPrefabsForInstantiation;

        private readonly List<GameObject> _extrasInstances = new();
        
        public void Setup(CraftingData craftingData,
            GameObject[] permittedGeneralChallenges, GameObject[] permittedResourceChallenges)
        {
            foreach (var extraPrefab in extraPrefabsForInstantiation)
            {
                var instance = Instantiate(extraPrefab, transform);
                _extrasInstances.Add(instance);
            }
            
            CraftingData = craftingData;
            Difficulty = craftingData.Recipe.difficultyConfig.difficulty;
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
            criticalFailCount++;
            
            OnCriticalFailReachedManager?.Invoke(this, criticalFailCount);
            
            if (criticalFailCount >= maxCriticalFails)
            {
                FailProduction();
            }
        }
        
        public delegate void OnBonusChangedHandler(float bonusModifier);

        public event OnBonusChangedHandler OnBonusChanged;

        public void ChangeBonus(float bonusModifier)
        {
            OnBonusChanged?.Invoke(bonusModifier);
        }

        public event EventHandler OnProductionFailed;
        
        private void FailProduction()
        {
            OnProductionFailed?.Invoke(this, null);
            
            EndProduction();
            
            // TODO: Send out some kind of struct holding session data in the event arguments?
        }

        public void FinishProductionSuccessfully()
        {
            OnProductionFinished?.Invoke(this, CraftingData);
            
            EndProduction();
        }

        public static event EventHandler<CraftingData> OnProductionFinished;
        
        private void EndProduction()
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

            foreach (var extraInstance in _extrasInstances)
            {
                Destroy(extraInstance);
            }
            
            Destroy(this);
        }
    }
}