using System;
using System.Linq;
using UnityEngine;

namespace Production.Challenges.General
{
    public abstract class GeneralBase<TConfig> : MonoBehaviour, IGeneralChallenge where TConfig : ConfigBase
    {
        [SerializeField] protected TConfig[] configs;
        
        protected ProductionSessionManager SessionManager;
        protected TConfig Config;

        protected virtual void Start()
        {
            SessionManager = GetComponentInParent<ProductionSessionManager>();
            Config = configs.FirstOrDefault(config => config.difficulty == SessionManager.difficulty);
        }

        protected virtual void FixedUpdate()
        {
            UpdateChallenge();
        }

        protected abstract void UpdateChallenge(); 
     
        public delegate void GeneralFailHandler();
        public event GeneralFailHandler GeneralFailed;
        
        protected void Fail()
        {
            GeneralFailed?.Invoke();
            Reset();
        }

        protected abstract void Reset();

        private GeneralFailHandler CreateFailHandler(Action onFailMethod)
        {
            return () => onFailMethod.Invoke();
        }
        
        public void AddMethodExecutedOnCriticalFail(Action onFailMethod)
        {
            GeneralFailed += CreateFailHandler(onFailMethod);
        }

        public void RemoveMethodExecutedOnCriticalFail(Action onFailMethod)
        {
            GeneralFailed -= CreateFailHandler(onFailMethod);
        }
    }

    public interface IGeneralChallenge
    {
        void AddMethodExecutedOnCriticalFail(Action onFailMethod);
        void RemoveMethodExecutedOnCriticalFail(Action onFailMethod);
    }
}