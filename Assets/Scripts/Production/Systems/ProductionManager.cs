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
        
        [HideInInspector] public ProductionSessionManager currentManager;
        
        public ProductionManagerConfig Config { get; private set; }
        
        [SerializeField] private ProductionChallengeRegistry challengeRegistry;
        [SerializeField] private GameObject sessionManagerPrefab;
        [SerializeField] private ProductionManagerConfig[] configs;

        private GameObject _currentManagerObject;
        
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
        
        public UnityEvent onProductionStarted, onProductionEnded;
        
        public void StartProduction(CraftingData craftingData)
        {
            Config = configs
                .FirstOrDefault(config => config.difficulty == craftingData.Recipe.difficultyConfig.difficulty);
            
            if (Config == null)
            {
                Debug.LogError($"No appropriate config found for {GetType().Name} " +
                               $"at {craftingData.Recipe.difficultyConfig}. " +
                               "Reverting to first available config.");
                
                Config = configs[0];
            }

            if (Config == null)
            {
                throw new Exception($"No config available for {GetType().Name}. Cannot start production.");
            }

            _currentManagerObject = Instantiate(sessionManagerPrefab, transform);
            currentManager = _currentManagerObject.GetComponent<ProductionSessionManager>();
            
            currentManager.Setup
            (
                craftingData,
                challengeRegistry
                    .GetActiveAndRestingGeneralChallenges
                    (
                        Random.Range
                        (
                            Config.minGeneralChallengesInSession, 
                            Config.maxGeneralChallengesInSession
                        )
                    ),
                challengeRegistry
                    .GetPermittedResourceChallenges
                    (
                        craftingData.Recipe.resources.Select(res => res.resource).ToArray()
                    )
            );

            currentManager.OnProductionEnded += SendOnProductionEndedEvent;
            
            onProductionStarted?.Invoke();
        }

        private void SendOnProductionEndedEvent(object sender, CraftingData cd)
        {
            currentManager.OnProductionEnded -= SendOnProductionEndedEvent;
            
            onProductionEnded?.Invoke();
        }
    }
    
    [Serializable]
    public class ProductionManagerConfig : ConfigBase
    {
        public int minGeneralChallengesInSession;
        public int maxGeneralChallengesInSession;
        [Range(-1, -99)] public int firstFailBonusLoss;
        [Range(-1, -99)] public int secondFailBonusLoss;
        [Range(1, 3)] public int maxFailCount;
    }
}