using System;
using System.Collections;
using UnityEngine;

namespace Production.Challenges.General.Temperature
{
    [Serializable]
    public class GenTemperature : GeneralBase<TemperatureConfig>
    {
        public float currentTemperature;

        protected override void Start()
        {
            base.Start();
            
            currentTemperature = Config.baseTemperature;
        }
        
        protected override void HandleUpdateLogic()
        {
            currentTemperature += Config.growthSpeed * Time.fixedDeltaTime;
        }

        protected override bool CheckWarningConditions()
        {
            return currentTemperature >= Config.warningThreshold;
        }

        protected override bool CheckFailConditions()
        {
            return currentTemperature >= Config.failThreshold;
        }
        
        public event EventHandler OnTemperatureAboveWarningThreshold;
        
        protected override void HandleWarningStart()
        {
            OnTemperatureAboveWarningThreshold?.Invoke(this, null);
        }
        
        public event EventHandler OnTemperatureBelowWarningThreshold;
        
        protected override void HandleWarningStop()
        {
            OnTemperatureBelowWarningThreshold?.Invoke(this, null);
        }

        protected override IEnumerator HandleResetLogic()
        {
            float startTime = Time.time;
            float elapsedTime = 0f;
            float temperatureAtResetStart = currentTemperature;

            while (elapsedTime < Config.resetWaitingTime)
            {
                float t = elapsedTime / Config.resetWaitingTime;

                currentTemperature = Mathf.Lerp(temperatureAtResetStart, Config.baseTemperature, t);

                elapsedTime = Time.time - startTime;

                yield return null;
            }

            currentTemperature = Config.baseTemperature;

            yield return null;
        }

        public void ReduceTemperature()
        {
            if (currentTemperature - Config.stepSize < Config.minTemperature)
            {
                currentTemperature = Config.minTemperature;
            }
            else
            {
                currentTemperature -= Config.stepSize;
            }
        }
    }

    [Serializable]
    public class TemperatureConfig : GeneralConfigBase
    {
        public int minTemperature;
        public int baseTemperature = 200;
        public float stepSize = 50;
        public float growthSpeed = 40;
    }
}