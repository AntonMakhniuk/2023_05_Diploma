using System;
using System.Collections;
using System.Linq;
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

            _difficulty = _sessionManager.difficulty;
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

        private void FixedUpdate()
        {
            if (_isBeingReset)
            {
                return;
            }
            
            HandlePerformanceConditions();
            HandleUpdateLogic();
        }

        private bool _isBeingReset;
        private bool _warningConditionsMet;
        private bool _failConditionsMet;
        private bool _isWarning;
        
        private void HandlePerformanceConditions()
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

        protected virtual void StartWarning()
        {
            _isWarning = true;
        }

        protected virtual void StopWarning()
        {
            _isWarning = false;
        }
     
        public delegate void GeneralFailHandler();
        public event GeneralFailHandler OnGeneralFail;


        private void Fail()
        {
            OnGeneralFail?.Invoke();
            
            StartCoroutine(ResetCoroutine());
        }
        
        private IEnumerator ResetCoroutine()
        {
            _isBeingReset = true;

            HandleResetLogic();

            yield return new WaitForSeconds(Config.resetWaitingTime);
            
            _isBeingReset = false;

            yield return null;
        }

        protected abstract void HandleResetLogic();
        
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