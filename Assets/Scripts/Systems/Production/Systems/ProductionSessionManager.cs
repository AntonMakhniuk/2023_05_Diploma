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
        private ProductionChallengeRegistry.GeneralChallengeData _generalChallengePrefabs;
        private GameObject[] _resourceChallengePrefabs;
        
        [HideInInspector] public int criticalFailCount;
        [HideInInspector] public CraftingData CraftingData;
        [HideInInspector] public bool productionIsOver;
        
        public Difficulty Difficulty { get; private set; }

        // currently running instances of session-specific challenges
        private readonly List<GameObject> _activeGeneralChallengeInstances = new();
        private readonly List<GameObject> _restingGeneralChallengeInstances = new();
        private readonly List<GameObject> _resourceChallengeInstances = new();

        [SerializeField] private GameObject[] extraPrefabsForInstantiation;
        [SerializeField] private GameObject productionEndOverlayPrefab;

        private readonly List<GameObject> _extrasInstances = new();
        private readonly GameObject _productionEndOverlayInstance;
        
        public void Setup(CraftingData craftingData,
            ProductionChallengeRegistry.GeneralChallengeData generalChallenges, 
            GameObject[] permittedResourceChallenges)
        {
            foreach (var extraPrefab in extraPrefabsForInstantiation)
            {
                var instance = Instantiate(extraPrefab, transform);
                _extrasInstances.Add(instance);
            }
            
            CraftingData = craftingData;
            Difficulty = craftingData.Recipe.difficultyConfig.difficulty;
            _generalChallengePrefabs = generalChallenges;
            _resourceChallengePrefabs = permittedResourceChallenges;
            
            // TODO: implement logic for choosing difficulty randomly
            
            foreach (GameObject generalPrefab in _generalChallengePrefabs.ActiveGeneralChallengePrefabs)
            {
                var instance = Instantiate(generalPrefab, transform);
                _activeGeneralChallengeInstances.Add(instance);
            }
            
            // TODO: introduce algorithm that spawns challenges over time
            
            foreach (IGeneralChallenge generalInstance in _activeGeneralChallengeInstances
                         .Select(ch => ch.GetComponent<IGeneralChallenge>())
                         .ToList())
            {
                generalInstance.Setup();
                generalInstance.SubscribeToOnGeneralFail(AddCriticalFail);
            }
            
            foreach (GameObject generalPrefab in _generalChallengePrefabs.RestingGeneralChallengePrefabs)
            {
                var instance = Instantiate(generalPrefab, transform);
                _restingGeneralChallengeInstances.Add(instance);
            }
            
            foreach (IGeneralChallenge generalInstance in _restingGeneralChallengeInstances
                         .Select(ch => ch.GetComponent<IGeneralChallenge>())
                         .ToList())
            {
                generalInstance.SetupAsInactive();
            }
        }

        public event EventHandler<int> OnCriticalFailReachedManager;
        
        private void AddCriticalFail()
        {
            criticalFailCount++;
            
            OnCriticalFailReachedManager?.Invoke(this, criticalFailCount);

            switch (criticalFailCount)
            {
                case 1:
                {
                    if (criticalFailCount < ProductionManager.Instance.Config.maxFailCount)
                    {
                        ChangeBonus(BonusSource.GeneralFail, ProductionManager.Instance.Config.firstFailBonusLoss);
                    }

                    break;
                }
                case 2:
                {
                    if (criticalFailCount < ProductionManager.Instance.Config.maxFailCount)
                    {
                        ChangeBonus(BonusSource.GeneralFail, ProductionManager.Instance.Config.secondFailBonusLoss);
                    }

                    break;
                }
            }
            
            if (criticalFailCount >= ProductionManager.Instance.Config.maxFailCount)
            {
                FailProduction();
            }
        }
        
        public delegate void OnBonusChangedHandler(float bonusModifier);

        public event OnBonusChangedHandler OnBonusChanged;

        public void ChangeBonus(BonusSource source, int bonusModifier)
        {
            CraftingData.UpdateModifier(source, bonusModifier);
            
            OnBonusChanged?.Invoke(bonusModifier);
        }

        public event EventHandler OnProductionFinished;
        public event EventHandler<CraftingData> OnProductionEnded;

        private void FailProduction()
        {
            CraftingData.FailProduction();
            
            productionIsOver = true;
            
            OnProductionFinished?.Invoke(this, null);
            
            DisplayFinalScoreCalculation();
        }

        public void FinishProductionSuccessfully()
        {
            CraftingData.ProductionFailed = false;
            productionIsOver = true;
            
            OnProductionFinished?.Invoke(this, null);
            
            DisplayFinalScoreCalculation();
        }

        private void DisplayFinalScoreCalculation()
        {
            Instantiate(productionEndOverlayPrefab, transform);
        }
        
        public void EndProduction()
        {
            OnProductionEnded?.Invoke(this, CraftingData);
            
            foreach (IGeneralChallenge generalInstance in _activeGeneralChallengeInstances
                         .Select(ch => ch.GetComponent<IGeneralChallenge>())
                         .ToList())
            {
                generalInstance.UnsubscribeFromOnGeneralFail(AddCriticalFail);
            }
            
            foreach (var challengeInstance in _activeGeneralChallengeInstances)
            {
                Destroy(challengeInstance);
            }

            foreach (var challengeInstance in _restingGeneralChallengeInstances)
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

            Destroy(_productionEndOverlayInstance);

            Destroy(gameObject);
        }
    }
}