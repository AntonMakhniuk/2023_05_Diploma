using System;
using UnityEngine;

namespace Production.Challenges.General.Temperature
{
    public class GenTemperature : GeneralBase<TemperatureConfig>
    {
        private float _currentTemperature;

        protected override void Start()
        {
            base.Start();
            
            _currentTemperature = Config.baseTemperature;
        }
        
        protected override void HandleUpdateLogic()
        {
            _currentTemperature += Config.growthSpeed * Time.fixedDeltaTime;
        }

        protected override bool CheckWarningConditions()
        {
            return _currentTemperature >= Config.warningThreshold;
        }

        protected override bool CheckFailConditions()
        {
            return _currentTemperature >= Config.failThreshold;
        }

        public delegate void TemperatureWarningHandler();
        public event TemperatureWarningHandler OnTemperatureWarningSurpassed;
        
        protected override void StartWarning()
        {
            base.StartWarning();
            
            OnTemperatureWarningSurpassed?.Invoke();
        }
        
        protected override void HandleResetLogic()
        {
            float currentTime = 0f;
            float temperatureAtResetStart = _currentTemperature;

            while (currentTime < Config.failureResetTime)
            {
                float t = currentTime / Config.failureResetTime;

                _currentTemperature = Mathf.Lerp(temperatureAtResetStart, Config.baseTemperature, t);

                currentTime += Time.deltaTime;
            }
        }

        public void ReduceTemperature()
        {
            if (_currentTemperature - Config.stepSize < Config.minTemperature)
            {
                _currentTemperature = Config.minTemperature;
            }
            else
            {
                _currentTemperature -= Config.stepSize;
            }
        }
    }

    [Serializable]
    public class TemperatureConfig : GeneralConfigBase
    {
        public int maxTemperature = 1000;
        public int minTemperature = 0;
        public int baseTemperature = 200;
        public float stepSize = 50;
        public float growthSpeed = 40;
        public float failureResetTime = 2.5f;
    }
}