using UnityEngine;
using UnityEngine.UI;

namespace Production.Challenges.General.Temperature
{
    public class TemperatureSliderCommunicator : MonoBehaviour
    {
        public GenTemperature associatedChallenge;
        public Slider associatedSlider;

        private TemperatureConfig _config;

        private void Start()
        {
            _config = associatedChallenge.Config;

            associatedSlider.maxValue = _config.failThreshold;
            associatedSlider.minValue = _config.minTemperature;
            associatedSlider.value = _config.baseTemperature;
        }

        private void FixedUpdate()
        {
            associatedSlider.value = associatedChallenge.currentTemperature;
        }
    }
}