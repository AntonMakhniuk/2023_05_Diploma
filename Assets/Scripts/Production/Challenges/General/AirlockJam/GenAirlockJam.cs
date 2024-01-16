using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Production.Challenges.General.AirlockJam
{
    public class GenAirlockJam : GeneralBase<AirlockJamConfig>
    {
        public float timePerBlink;
        [FormerlySerializedAs("blinkCount")] public int totalBlinkCount;
        
        private AirlockButton[] _buttons;
        private int _turnedOffButtonsCount;
        private bool _isBeingReset;
        
        public delegate void AirlockJamWarningHandler();
        public event AirlockJamWarningHandler AirlockJamWarningSurpassed;
        
        protected override void UpdateChallenge()
        {
            if (_isBeingReset)
            {
                return;
            }
            
            _turnedOffButtonsCount = 0;
            
            foreach (var airlockButton in _buttons)
            {
                if (airlockButton.IsTurnedOn == false)
                {
                    _turnedOffButtonsCount++;
                }
            }

            if (_turnedOffButtonsCount >= Config.failThreshold)
            {
                Fail();
            }
            else if (_turnedOffButtonsCount >= Config.warningThreshold)
            {
                AirlockJamWarningSurpassed?.Invoke();
            }
        }

        protected override void Reset()
        {
            if (_isBeingReset)
            {
                return;
            }

            StartCoroutine(ResetButtons());
        }

        private IEnumerator ResetButtons()
        {
            _isBeingReset = true;

            foreach (var airlockButton in _buttons)
            {
                StartCoroutine(airlockButton.Blink(timePerBlink, totalBlinkCount));
            }
            
            yield return new WaitForSeconds(timePerBlink * totalBlinkCount);
            
            _isBeingReset = false;

            yield return null;
        }
    }

    [Serializable]
    public class AirlockJamConfig : ConfigBase
    {
        public int warningThreshold = 6;
        public int failThreshold = 12;
    }
}