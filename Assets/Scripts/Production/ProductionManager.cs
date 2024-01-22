using System;
using System.Linq;
using DefaultNamespace;
using Production.Challenges;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Production
{
    public class ProductionManager : MonoBehaviour
    {
        private static ProductionManager _instance;
        
        [SerializeField] private ProductionChallengeRegistry challengeRegistry;
        [SerializeField] private ProductionSessionManager sessionManagerPrefab;
        [SerializeField] private ProductionManagerConfig[] configs; 
        
        private ProductionSessionManager _currentManager;
        private ProductionManagerConfig _currentConfig;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartProductionHardcoded()
        {
            StartProduction(Difficulty.Normal, new ResourcePlaceholder[]{});
        }
        
        // TODO: Add proper resources to method
        
        public void StartProduction(Difficulty difficulty, ResourcePlaceholder[] resources)
        {
            _currentConfig = configs.FirstOrDefault(config => config.difficulty == difficulty);
            
            if (_currentConfig == null)
            {
                Debug.LogError($"No appropriate config found for {GetType().Name} at {difficulty}. " +
                               "Reverting to first available config.");
                
                _currentConfig = configs[0];
            }

            if (_currentConfig == null)
            {
                throw new Exception($"No config available for {GetType().Name}. Cannot start production.");
            }

            _currentManager = Instantiate(sessionManagerPrefab, transform);
            
            _currentManager.Setup
            (
                difficulty, 
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
                        resources
                    )
            );
        }
    }

    [Serializable]
    public class ProductionManagerConfig : ConfigBase
    {
        public int minGeneralChallengesInSession;
        public int maxGeneralChallengesInSession;
    }
}