using System.Collections;
using UnityEngine;

namespace Production.Challenges
{
    public class GenTemperature : GeneralBase
    {
        [SerializeField] private int maxTemperature = 1000;
        [SerializeField] private int minTemperature = 0;
        [SerializeField] private int baseTemperature = 200;
        [SerializeField] private int warningThreshold = 500;
        [SerializeField] private float stepSize = 50;
        [SerializeField] private float growthSpeed = 40;
        [SerializeField] private float failureResetTime = 2.5f;
        
        private float _currentTemperature;
        private bool _isBeingReset;
        
        public delegate void TemperatureWarningHandler();
        public event TemperatureWarningHandler TemperatureWarningSurpassed;

        private void Start()
        {
            _currentTemperature = baseTemperature;
            _isBeingReset = false;
        }

        protected override void UpdateChallenge()
        {
            if (_isBeingReset)
            {
                return;
            }
            
            _currentTemperature += growthSpeed * Time.fixedDeltaTime;

            if (_currentTemperature >= maxTemperature)
            {
                Fail();
            }
            else if (_currentTemperature >= warningThreshold)
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
            
            if (_currentTemperature - stepSize < minTemperature)
            {
                _currentTemperature = minTemperature;
            }
            else
            {
                _currentTemperature -= stepSize;
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

            while (currentTime < failureResetTime)
            {
                float t = currentTime / failureResetTime;

                _currentTemperature = Mathf.Lerp(temperatureAtResetStart, baseTemperature, t);

                yield return null;
            }

            _isBeingReset = false;
        }
    }
}