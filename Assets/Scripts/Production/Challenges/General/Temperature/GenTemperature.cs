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
        
        protected override void UpdateChallengeElements()
        {
            currentTemperature += Config.growthSpeed * updateRate;
        }

        protected override int GetNumberOfWarnings()
        {
            return currentTemperature >= Config.warningTemperature ? 1 : 0;
        }

        protected override int GetNumberOfFails()
        {
            return currentTemperature >= Config.failTemperature ? 1 : 0;
        }

        protected override IEnumerator ResetLogicCoroutine()
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
        public int maxTemperature;
        public int failTemperature;
        public int warningTemperature;
        public int minTemperature;
        public int baseTemperature = 200;
        public float stepSize = 50;
        public float growthSpeed = 40;
    }
}