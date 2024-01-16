using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Production.Challenges
{
    public class GenTemperature : GeneralBase<TemperatureConfig>
    {
        private float _currentTemperature;
        private bool _isBeingReset;
        
        public delegate void TemperatureWarningHandler();
        public event TemperatureWarningHandler TemperatureWarningSurpassed;

        protected override void Start()
        {
            base.Start();
            _currentTemperature = Config.baseTemperature;
            _isBeingReset = false;
        }

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
                TemperatureWarningSurpassed?.Invoke();
            }
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