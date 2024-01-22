using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Production.Challenges.General.AirlockJam
{
    public class GenAirlockJam : GeneralBase<AirlockJamConfig>
    {
        private AirlockButton[] _allButtons;
        private List<AirlockButton> _enabledButtons;
        private List<AirlockButton> _disabledButtons;
        
        // TODO: Add instantiation of buttons

        protected override void Start()
        {
            base.Start();

            _enabledButtons = new List<AirlockButton>(_allButtons);
            _disabledButtons = new List<AirlockButton>();
        }

        protected override bool CheckWarningConditions()
        {
            return _disabledButtons.Count >= Config.warningThreshold;
        }

        protected override bool CheckFailConditions()
        {
            return _disabledButtons.Count >= Config.failThreshold;
        }
        
        private bool _disablingIsOnCooldown;
        
        protected override void HandleUpdateLogic()
        {
            if (!_disablingIsOnCooldown)
            {
                StartCoroutine(DisableButtons());
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
        
        public event EventHandler OnAirlockJamAboveWarningThreshold;
        
        protected override void HandleWarningStart()
        {
            OnAirlockJamAboveWarningThreshold?.Invoke(this, null);
        }

        public event EventHandler OnAirlockJamBelowWarningThreshold;
        
        protected override void HandleWarningStop()
        {
            OnAirlockJamBelowWarningThreshold?.Invoke(this, null);
        }

        protected override IEnumerator HandleResetLogic()
        {
            foreach (var airlockButton in _allButtons)
            {
                StartCoroutine(airlockButton.Blink(Config.singleBlinkTime, Config.totalBlinkCount));

                yield return null;
            }
        }
    }

    [Serializable]
    public class AirlockJamConfig : GeneralConfigBase
    {
        public int totalBlinkCount = 3;
        public float singleBlinkTime = 0.5f;
        public int minDisabledButtonsPerTurn = 2;
        public int maxDisabledButtonsPerTurn = 4;
        public float disableCooldown = 2f;
    }
}