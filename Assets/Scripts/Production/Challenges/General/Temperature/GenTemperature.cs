using System;
using System.Collections;
using UnityEngine;

namespace Production.Challenges.General.Temperature
{
    public class GenTemperature : GeneralBase<TemperatureConfig>
    {
        private float _currentTemperature;
        private bool _isBeingReset;
        private bool _isWarning;

        protected override void Start()
        {
            base.Start();
            _currentTemperature = Config.baseTemperature;
        }

        public delegate void TemperatureWarningHandler();
        public event TemperatureWarningHandler TemperatureWarningSurpassed;
        
        protected override void UpdateChallenge()
        {
            if (_isBeingReset)
            {
                return;
            }
            
            _currentTemperature += Config.growthSpeed * Time.fixedDeltaTime;

            if (_currentTemperature >= Config.maxTemperature)
            {
                Fail();
            }
            else if (_currentTemperature >= Config.warningThreshold)
            {
                if (!_isWarning)
                {
                    StartCoroutine(Warn());
                }
            }
        }

        private IEnumerator Warn()
        {
            _isWarning = true;
            
            TemperatureWarningSurpassed?.Invoke();

            yield return new WaitForSeconds(Config.warningLength);

            _isWarning = false;
            
            yield return null;
        }
        
        public void ReduceTemperature()
        {
            if (_isBeingReset)
            {
                return;
            }
            
            if (_currentTemperature - Config.stepSize < Config.minTemperature)
            {
                _currentTemperature = Config.minTemperature;
            }
            else
            {
                _currentTemperature -= Config.stepSize;
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

            while (currentTime < Config.failureResetTime)
            {
                float t = currentTime / Config.failureResetTime;

                _currentTemperature = Mathf.Lerp(temperatureAtResetStart, Config.baseTemperature, t);

                currentTime += Time.deltaTime;
            }

            _isBeingReset = false;

            yield return null;
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
        public float warningLength = 1f;
    }
}