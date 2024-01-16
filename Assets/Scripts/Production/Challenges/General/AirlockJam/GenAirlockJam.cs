using System;
using System.Collections;
using UnityEngine;

namespace Production.Challenges.General.AirlockJam
{
    public class GenAirlockJam : GeneralBase<AirlockJamConfig>
    {
        public float timePerBlink;
        public int totalBlinkCount;
        
        private AirlockButton[] _buttons;
        private int _turnedOffButtonsCount;
        private bool _isBeingReset;
        private bool _isWarning;
        
        public delegate void AirlockJamWarningHandler();
        public event AirlockJamWarningHandler AirlockJamWarningSurpassed;
        
        // TODO: Add Start method and instantiation of buttons
        
        protected override void UpdateChallenge()
        {
            if (_isBeingReset)
            {
                return;
            }
            
            _turnedOffButtonsCount = 0;
            
            // TODO: write button disabling logic
            
            foreach (var airlockButton in _buttons)
            {
                if (airlockButton.isTurnedOn == false)
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
                if (!_isWarning)
                {
                    StartCoroutine(Warn());
                }
            }
        }

        private IEnumerator Warn()
        {
            _isWarning = true;
            
            AirlockJamWarningSurpassed?.Invoke();

            yield return new WaitForSeconds(Config.warningLength);

            _isWarning = false;
            
            yield return null;
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
        public float warningLength = 1f;
    }
}