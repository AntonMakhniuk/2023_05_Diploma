using System;
using System.Linq;
using Production.Challenges;
using Production.Crafting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Production.Systems
{
    public class ProductionManager : MonoBehaviour
    {
        // TODO: Make the attached game object be disabled until it is needed for performance (it has a canvas)
        
        public static ProductionManager Instance;
        
        [SerializeField] private ProductionChallengeRegistry challengeRegistry;
        [SerializeField] private GameObject sessionManagerPrefab;
        [SerializeField] private ProductionManagerConfig[] configs;

        private GameObject _currentManagerObject;
        private ProductionSessionManager _currentManagerScript;
        private ProductionManagerConfig _currentConfig;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public UnityEvent onProductionStarted;
        
        public void StartProduction(CraftingData craftingData)
        {
            _currentConfig = configs
                .FirstOrDefault(config => config.difficulty == craftingData.Recipe.difficultyConfig.difficulty);
            
            if (_currentConfig == null)
            {
                Debug.LogError($"No appropriate config found for {GetType().Name} " +
                               $"at {craftingData.Recipe.difficultyConfig}. " +
                               "Reverting to first available config.");
                
                _currentConfig = configs[0];
            }

            if (_currentConfig == null)
            {
                throw new Exception($"No config available for {GetType().Name}. Cannot start production.");
            }

            _currentManagerObject = Instantiate(sessionManagerPrefab, transform);
            _currentManagerScript = _currentManagerObject.GetComponent<ProductionSessionManager>();
            
            _currentManagerScript.Setup
            (
                craftingData,
                challengeRegistry
                    .GetNumberOfRandomGeneralChallenges
                    (
                        Random.Range
                        (
                            _currentConfig.minGeneralChallengesInSession, 
                            _currentConfig.maxGeneralChallengesInSession
                        )
                    ),
                challengeRegistry
                    .GetPermittedResourceChallenges
                    (
                        craftingData.Recipe.resources.Select(res => res.resource).ToArray()
                    )
            );

            _currentManagerScript.OnProductionFailed += DestroyCurrentManager;
            
            onProductionStarted?.Invoke();
        }
        
        private void DestroyCurrentManager(object sender, EventArgs args)
        {
            _currentManagerScript.OnProductionFailed -= DestroyCurrentManager;
            
            Destroy(_currentManagerObject);
        }
    }
    
    [Serializable]
    public class ProductionManagerConfig : ConfigBase
    {
        public int minGeneralChallengesInSession;
        public int maxGeneralChallengesInSession;
    }
}