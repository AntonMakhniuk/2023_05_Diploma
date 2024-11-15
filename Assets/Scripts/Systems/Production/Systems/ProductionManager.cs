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
        
        public static void StartProduction(CraftingData craftingData)
        {
            Instance.Config = Instance.configs
                .FirstOrDefault(config => config.difficulty == craftingData.Recipe.difficultyConfig.difficulty);
            
            if (Instance.Config == null)
            {
                Debug.LogError($"No appropriate config found for {Instance.GetType().Name} " +
                               $"at {craftingData.Recipe.difficultyConfig}. " +
                               "Reverting to first available config.");
                
                Instance.Config = Instance.configs[0];
            }

            if (Instance.Config == null)
            {
                throw new Exception($"No config available for {Instance.GetType().Name}. Cannot start production.");
            }

            Instance._currentManagerObject = Instantiate(Instance.sessionManagerPrefab, Instance.transform);
            Instance.currentManager = Instance._currentManagerObject.GetComponent<ProductionSessionManager>();
            
            Instance.currentManager.Setup
            (
                craftingData,
                Instance.challengeRegistry
                    .GetActiveAndRestingGeneralChallenges
                    (
                        Random.Range
                        (
                            Instance.Config.minGeneralChallengesInSession, 
                            Instance.Config.maxGeneralChallengesInSession
                        )
                    ),
                Instance.challengeRegistry
                    .GetPermittedResourceChallenges
                    (
                        craftingData.Recipe.resources.Select(res => res.resourceData).ToArray()
                    )
            );

            Instance.currentManager.OnProductionEnded += Instance.SendOnProductionEndedEvent;
            
            Instance.onProductionStarted?.Invoke();
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