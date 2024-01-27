using System;
using System.Linq;
using Production.Challenges;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Production.Systems
{
    public class ProductionManager : MonoBehaviour
    {
        private static ProductionManager _instance;
        
        [SerializeField] private ProductionChallengeRegistry challengeRegistry;
        [SerializeField] private GameObject sessionManagerPrefab;
        [SerializeField] private ProductionManagerConfig[] configs;

        private GameObject _currentManagerObject;
        private ProductionSessionManager _currentManagerScript;
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

            _currentManagerObject = Instantiate(sessionManagerPrefab, transform);
            _currentManagerScript = _currentManagerObject.GetComponent<ProductionSessionManager>();
            
            _currentManagerScript.Setup
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

            _currentManagerScript.OnProductionFailed += DestroyCurrentManager;
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