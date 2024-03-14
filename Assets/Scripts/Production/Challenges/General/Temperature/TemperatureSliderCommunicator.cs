using System.Collections.Generic;
using Miscellaneous;
using ThirdParty.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Production.Challenges.General.Temperature
{
    public class TemperatureSliderCommunicator : MonoBehaviour
    {
        public GenTemperature associatedChallenge;
        public Slider associatedSlider;
        public Image sliderBackground;

        public Color32 dangerZoneColor;
        public Color32 warningZoneColor;
        public Color32 safeZoneColor;
        
        private TemperatureConfig _config;

        private void Start()
        {
            _config = associatedChallenge.Config;

            associatedSlider.maxValue = _config.failTemperature;
            associatedSlider.minValue = _config.minTemperature;
            associatedSlider.value = _config.baseTemperature;

            List<CustomGradientKey> colorKeys = new List<CustomGradientKey>();
            
            colorKeys.Add(new CustomGradientKey(safeZoneColor, 0f));
            colorKeys.Add(new CustomGradientKey(safeZoneColor, 
                (float)_config.warningTemperature / _config.maxTemperature));
            colorKeys.Add(new CustomGradientKey(warningZoneColor, 
                (float)_config.warningTemperature / _config.maxTemperature + 0.001f));
            colorKeys.Add(new CustomGradientKey(warningZoneColor,
                (float)_config.failTemperature / _config.maxTemperature));
            colorKeys.Add(new CustomGradientKey(dangerZoneColor,
                (float)_config.failTemperature / _config.maxTemperature + 0.001f));
            colorKeys.Add(new CustomGradientKey(dangerZoneColor, 1f));
            
            Utility.CreateGradientSpriteAndApplyToSlider(associatedSlider, sliderBackground, colorKeys.ToArray());
            
            InvokeRepeating(nameof(UpdateSlider), 0, associatedChallenge.updateRate);
        }

        private void UpdateSlider()
        {
            associatedSlider.value = associatedChallenge.currentTemperature;
        }
    }
}