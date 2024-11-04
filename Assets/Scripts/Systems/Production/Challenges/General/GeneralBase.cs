using System;
using System.Collections;
using System.Linq;
using Production.Crafting;
using Production.Systems;
using UnityEngine;

namespace Production.Challenges.General
{
    public abstract class GeneralBase<TConfig> : MonoBehaviour, IGeneralChallenge where TConfig : GeneralConfigBase
    {
        [SerializeField] protected GameObject[] interactiveElementsParents;
        [SerializeField] protected TConfig[] configs;
        [SerializeField] protected GeneralType challengeType;
        
        private ProductionSessionManager _sessionManager;
        private Difficulty _difficulty;
        
        public TConfig Config { get; private set; }
        public float updateRate = 0.03333f;
        public bool isActive;
        
        protected bool UpdateIsPaused;
        
        private bool _isBeingReset;

        public virtual void Setup()
        {
            _sessionManager = GetComponentInParent<ProductionSessionManager>();

            if (_sessionManager == null)
            {
                throw new Exception($"'{nameof(_sessionManager)}' is null. " +
                                    $"{GetType().Name} has been instantiated outside ProductionSessionManager");
            }
            
            // TODO: Change the difficulty to be given by the session manager instead of taken from it

            _difficulty = _sessionManager.Difficulty;
            Config = configs.FirstOrDefault(config => config.difficulty == _difficulty);

            if (Config == null)
            {
                Debug.LogError($"No appropriate config found for {GetType().Name} at {_difficulty}. " +
                               "Reverting to first available config.");
                
                Config = configs[0];
            }

            if (Config == null)
            {
                throw new Exception($"No config available for {GetType().Name}. Cannot start challenge.");
            }

            isActive = true;
            
            ChangeInteractive(true);
            
            _sessionManager.OnProductionFinished += ForceResetAndStop;
            
            InvokeRepeating(nameof(UpdateChallenge), 0, updateRate);
        }

        public void SetupAsInactive()
        {
            _sessionManager = GetComponentInParent<ProductionSessionManager>();

            if (_sessionManager == null)
            {
                throw new Exception($"'{nameof(_sessionManager)}' is null. " +
                                    $"{GetType().Name} has been instantiated outside ProductionSessionManager");
            }
            
            isActive = false;
            
            ChangeInteractive(false);
        }

        protected abstract void ChangeInteractive(bool newState);
        
        private void UpdateChallenge()
        {
            if (_isBeingReset || _sessionManager.productionIsOver)
            {
                return;
            }
            
            CheckFailConditions();
            CheckWarningConditions();

            if (UpdateIsPaused)
            {
                return;
            }
            
            UpdateChallengeElements();
        }

        public delegate void GeneralElementEventHandler(int numberOfFails, GeneralType generalType);

        public event GeneralElementEventHandler OnGeneralElementFail;
        
        private void CheckFailConditions()
        {
            int numOfFails = GetNumberOfFails();
            
            if (numOfFails >= Config.failThreshold)
            {
                Fail();
            }

            OnGeneralElementFail?.Invoke(numOfFails, challengeType);
        }
        
        protected abstract int GetNumberOfFails();
        
        public delegate void GeneralFailHandler();
        public event GeneralFailHandler OnGeneralFail;
        public event EventHandler OnGeneralReset;
        
        private void Fail()
        {
            if (_sessionManager.productionIsOver)
            {
                return;
            }
            
            OnGeneralFail?.Invoke();
            
            _isBeingReset = true;
            
            StartCoroutine(ResetCoroutine());
        }

        private void ForceResetAndStop(object sender, EventArgs e)
        {
            if (!_isBeingReset)
            {
                StartCoroutine(ResetCoroutine());
            }
        }
        
        private IEnumerator ResetCoroutine()
        {
            yield return StartCoroutine(ResetLogicCoroutine());

            OnGeneralReset?.Invoke(this, EventArgs.Empty);
            
            _isBeingReset = false;
        }

        protected abstract IEnumerator ResetLogicCoroutine();

        public event GeneralElementEventHandler OnGeneralElementWarning;
        
        private void CheckWarningConditions()
        {
            OnGeneralElementWarning?.Invoke(GetNumberOfWarnings(), challengeType);
        }
        
        protected abstract int GetNumberOfWarnings();
        
        protected abstract void UpdateChallengeElements();

        protected IEnumerator PauseUpdateForSeconds(float timeInSeconds)
        {
            UpdateIsPaused = true;
            
            yield return new WaitForSeconds(timeInSeconds);

            UpdateIsPaused = false;
        }
        
        private static GeneralFailHandler CreateFailHandlerDelegate(Action onFailMethod)
        {
            return onFailMethod.Invoke;
        }
        
        public void SubscribeToOnGeneralFail(Action onFailMethod)
        {
            OnGeneralFail += CreateFailHandlerDelegate(onFailMethod);
        }

        public void UnsubscribeFromOnGeneralFail(Action onFailMethod)
        {
            OnGeneralFail -= CreateFailHandlerDelegate(onFailMethod);
        }
        
        private void OnDestroy()
        {
            _sessionManager.OnProductionFinished -= ForceResetAndStop;
            
            StopAllCoroutines();
        }
    }

    public interface IGeneralChallenge
    {
        void SubscribeToOnGeneralFail(Action onFailMethod);
        void UnsubscribeFromOnGeneralFail(Action onFailMethod);

        void Setup();

        void SetupAsInactive();
    }

    public enum GeneralType
    {
        Temperature, RodPositioning, AirlockJam, CoreSegmentation
    }
}