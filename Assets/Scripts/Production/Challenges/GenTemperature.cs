using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Production.Challenges
{
    public class GenTemperature : GeneralBase
    {
        [SerializeField] private TemperatureConfig[] configs;

        private TemperatureConfig _config;
        private ProductionSessionManager _sessionManager;
        private float _currentTemperature;
        private bool _isBeingReset;
        
        public delegate void TemperatureWarningHandler();
        public event TemperatureWarningHandler TemperatureWarningSurpassed;

        private void Start()
        {
            _sessionManager = GetComponentInParent<ProductionSessionManager>();
            _config = configs.FirstOrDefault(config => config.difficulty == _sessionManager.difficulty);
            _currentTemperature = _config.baseTemperature;
            _isBeingReset = false;
        }

        protected override void UpdateChallenge()
        {
            if (_isBeingReset)
            {
                return;
            }
            
            _currentTemperature += _config.growthSpeed * Time.fixedDeltaTime;

            if (_currentTemperature >= _config.maxTemperature)
            {
                Fail();
            }
            else if (_currentTemperature >= _config.warningThreshold)
            {
                TemperatureWarningSurpassed?.Invoke();
            }
        }

        public void ReduceTemperature()
        {
            if (_isBeingReset)
            {
                return;
            }
            
            if (_currentTemperature - _config.stepSize < _config.minTemperature)
            {
                _currentTemperature = _config.minTemperature;
            }
            else
            {
                _currentTemperature -= _config.stepSize;
            }
        }
        
        protected override void Reset()
        {
            if (_isBeingReset)
            {
                return;
            }
            
            StartCoroutine(ResetTemperature());
        }

        private IEnumerator ResetTemperature()
        {
            _isBeingReset = true;
            
            float currentTime = 0f;
            float temperatureAtResetStart = _currentTemperature;

            while (currentTime < _config.failureResetTime)
            {
                float t = currentTime / _config.failureResetTime;

                _currentTemperature = Mathf.Lerp(temperatureAtResetStart, _config.baseTemperature, t);

                yield return null;
            }

            _isBeingReset = false;
        }
    }

    [Serializable]
    public class TemperatureConfig : ConfigBase
    {
        public int maxTemperature = 1000;
        public int minTemperature = 0;
        public int baseTemperature = 200;
        public int warningThreshold = 500;
        public float stepSize = 50;
        public float growthSpeed = 40;
        public float failureResetTime = 2.5f;
    }
}