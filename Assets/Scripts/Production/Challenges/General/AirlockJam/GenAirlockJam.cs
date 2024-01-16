using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Production.Challenges.General.AirlockJam
{
    public class GenAirlockJam : GeneralBase<AirlockJamConfig>
    {
        public float timePerBlink;
        public int totalBlinkCount;
        
        private AirlockButton[] _allButtons;
        private List<AirlockButton> _enabledButtons;
        private List<AirlockButton> _disabledButtons;
        private bool _isBeingReset;
        private bool _isWarning;
        private bool _disablingIsOnCooldown;
        
        public delegate void AirlockJamWarningHandler();
        public event AirlockJamWarningHandler AirlockJamWarningSurpassed;
        
        // TODO: Add Start method and instantiation of buttons

        protected override void Start()
        {
            base.Start();

            _enabledButtons = new List<AirlockButton>(_allButtons);
            _disabledButtons = new List<AirlockButton>();
        }

        protected override void UpdateChallenge()
        {
            if (_isBeingReset)
            {
                return;
            }

            if (!_disablingIsOnCooldown)
            {
                StartCoroutine(DisableButtons());
            }

            if (_disabledButtons.Count >= Config.failThreshold)
            {
                Fail();
            }
            else if (_disabledButtons.Count >= Config.warningThreshold)
            {
                if (!_isWarning)
                {
                    StartCoroutine(Warn());
                }
            }
        }

        private IEnumerator DisableButtons()
        {
            _disablingIsOnCooldown = true;
            
            int numberOfDisabledButtons =
                Random.Range(Config.minDisabledButtonsPerTurn, Config.maxDisabledButtonsPerTurn);
            
            for (int i = 0; i < numberOfDisabledButtons; i++)
            {
                var chosenButton = _enabledButtons[Random.Range(0, _enabledButtons.Count)];
                chosenButton.TurnOff();
                _enabledButtons.Remove(chosenButton);
                _disabledButtons.Add(chosenButton);
            }

            yield return new WaitForSeconds(Config.disableCooldown);

            _disablingIsOnCooldown = false;

            yield return null;
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

            foreach (var airlockButton in _allButtons)
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
        public int minDisabledButtonsPerTurn = 2;
        public int maxDisabledButtonsPerTurn = 4;
        public float disableCooldown = 2f;
    }
}