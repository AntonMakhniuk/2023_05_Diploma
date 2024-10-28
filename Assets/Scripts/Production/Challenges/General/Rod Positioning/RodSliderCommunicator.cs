using System;
using System.Collections.Generic;
using Miscellaneous;
using ThirdParty.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Production.Challenges.General.Rod_Positioning
{
    public class RodSliderCommunicator : MonoBehaviour
    {
        public RodLever associatedLever;
        public Slider associatedSlider;
        public Image sliderBackground;

        public Color32 dangerZoneColor;
        public Color32 warningZoneColor;
        public Color32 safeZoneColor;

        private GenRodPositioning _associatedChallenge;

        private void Start()
        {
            _associatedChallenge = GetComponentInParent<GenRodPositioning>();
            
            if (_associatedChallenge == null)
            {
                throw new Exception("RodSliderCommunicator has been instantiated outside of GenRodPositioning");
            }
            
            // TODO: figure out why this is here
            
            associatedSlider.maxValue = 1f;
            associatedSlider.minValue = 0f;
            associatedSlider.value = associatedLever.currentPosition;

            List<CustomGradientKey> colorKeys = new List<CustomGradientKey>();

            switch (associatedLever.dangerZoneType)
            {
                case LeverDangerZoneType.TopOnly:
                {
                    colorKeys.Add(new CustomGradientKey(safeZoneColor, 0f));
                    colorKeys.Add(new CustomGradientKey(safeZoneColor, 
                        associatedLever.safeRangeEnd));
                    colorKeys.Add(new CustomGradientKey(warningZoneColor, 
                        associatedLever.safeRangeEnd + 0.001f));
                    colorKeys.Add(new CustomGradientKey(warningZoneColor,
                        associatedLever.dangerZoneSingleStart));
                    colorKeys.Add(new CustomGradientKey(dangerZoneColor,
                        associatedLever.dangerZoneSingleStart + 0.001f));
                    colorKeys.Add(new CustomGradientKey(dangerZoneColor, 1f));

                    break;
                }
                case LeverDangerZoneType.BottomOnly:
                {
                    colorKeys.Add(new CustomGradientKey(safeZoneColor, 1f));
                    colorKeys.Add(new CustomGradientKey(safeZoneColor, 
                        associatedLever.safeRangeStart));
                    colorKeys.Add(new CustomGradientKey(warningZoneColor, 
                        associatedLever.safeRangeStart - 0.001f));
                    colorKeys.Add(new CustomGradientKey(warningZoneColor,
                        associatedLever.dangerZoneSingleStart));
                    colorKeys.Add(new CustomGradientKey(dangerZoneColor,
                        associatedLever.dangerZoneSingleStart - 0.001f));
                    colorKeys.Add(new CustomGradientKey(dangerZoneColor, 0f));

                    break;
                }
                case LeverDangerZoneType.Both:
                {
                    colorKeys.Add(new CustomGradientKey(dangerZoneColor, 0f));
                    colorKeys.Add(new CustomGradientKey(dangerZoneColor, 
                        associatedLever.dangerZoneSingleStart));
                    colorKeys.Add(new CustomGradientKey(warningZoneColor, 
                        associatedLever.dangerZoneSingleStart + 0.001f));
                    colorKeys.Add(new CustomGradientKey(warningZoneColor,
                        associatedLever.safeRangeStart));
                    colorKeys.Add(new CustomGradientKey(safeZoneColor,
                        associatedLever.safeRangeStart + 0.001f));
                    colorKeys.Add(new CustomGradientKey(safeZoneColor, 
                        associatedLever.safeRangeEnd - 0.001f));
                    colorKeys.Add(new CustomGradientKey(warningZoneColor, 
                        associatedLever.safeRangeEnd));
                    colorKeys.Add(new CustomGradientKey(warningZoneColor,
                        associatedLever.dangerZoneBothStart - 0.001f));
                    colorKeys.Add(new CustomGradientKey(dangerZoneColor,
                        associatedLever.dangerZoneBothStart));
                    colorKeys.Add(new CustomGradientKey(dangerZoneColor, 1f));

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            UtilityFunctions.CreateGradientSpriteAndApplyToSlider(associatedSlider, sliderBackground, colorKeys.ToArray());
            
            InvokeRepeating(nameof(UpdateSlider), 0, _associatedChallenge.updateRate);
        }

        // TODO: Potentially expensive method?
        
        private void UpdateSlider()
        {
            associatedSlider.value = associatedLever.currentPosition;
        }
    }
}