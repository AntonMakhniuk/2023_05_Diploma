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
        [SerializeField] protected TConfig[] configs;

        private ProductionSessionManager _sessionManager;
        private Difficulty _difficulty;
        
        public TConfig Config { get; private set; }

        protected virtual void Start()
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
        }

        private bool _isBeingReset;
        
        private void FixedUpdate()
        {
            if (_isBeingReset)
            {
                return;
            }
            
            HandlePerformanceConditionsCheck();
            HandleUpdateLogic();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
        
        private bool _warningConditionsMet;
        private bool _failConditionsMet;
        private bool _isWarning;
        
        private void HandlePerformanceConditionsCheck()
        {
            _warningConditionsMet = CheckWarningConditions();
            _failConditionsMet = CheckFailConditions();

            if (_failConditionsMet)
            {
                Fail();
            }
            else if (!_isWarning && _warningConditionsMet)
            {
                StartWarning();
            }
            else if (_isWarning && !_warningConditionsMet)
            {
                StopWarning();
            }
        }

        protected abstract void HandleUpdateLogic();
        
        protected abstract bool CheckWarningConditions();
        
        protected abstract bool CheckFailConditions();

        private void StartWarning()
        {
            _isWarning = true;
            
            HandleWarningStart();
        }

        protected abstract void HandleWarningStart();

        private void StopWarning()
        {
            _isWarning = false;
            
            HandleWarningStop();
        }

        protected abstract void HandleWarningStop();
     
        public delegate void GeneralFailHandler();
        public event GeneralFailHandler OnGeneralFail;


        private void Fail()
        {
            OnGeneralFail?.Invoke();
            
            StartCoroutine(ResetCoroutine());
        }
        
        // TODO: Fix issue where reset logic can be executed for longer than the resetWaitingTime
        
        private IEnumerator ResetCoroutine()
        {
            _isBeingReset = true;

            StartCoroutine(HandleResetLogic());

            yield return new WaitForSeconds(Config.resetWaitingTime);
            
            _isBeingReset = false;

            yield return null;
        }

        protected abstract IEnumerator HandleResetLogic();
        
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
    }

    public interface IGeneralChallenge
    {
        void SubscribeToOnGeneralFail(Action onFailMethod);
        void UnsubscribeFromOnGeneralFail(Action onFailMethod);
    }
}