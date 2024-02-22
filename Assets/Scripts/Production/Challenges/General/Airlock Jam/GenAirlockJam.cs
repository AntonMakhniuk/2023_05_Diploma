using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Production.Challenges.General.Airlock_Jam
{
    public class GenAirlockJam : GeneralBase<AirlockJamConfig>
    {
        [SerializeField] private GridLayoutGroup buttonGrid;
        [SerializeField] private GameObject buttonPrefab;
        
        private List<AirlockButton> _allButtons;
        private List<AirlockButton> _enabledButtons;
        private List<AirlockButton> _disabledButtons;

        protected override void Start()
        {
            base.Start();

            _allButtons = new List<AirlockButton>();
            _disabledButtons = new List<AirlockButton>();
            
            InstantiateButtons();

            foreach (var airlockButton in _allButtons)
            {
                airlockButton.OnButtonTurnedOff += ChangeButtonStatusToOff;
                airlockButton.OnButtonTurnedOn += ChangeButtonStatusToOn;
            }
            
            _enabledButtons = new List<AirlockButton>(_allButtons);

            // Added so that buttons don't get disabled as soon as the challenge starts
            StartCoroutine(PauseUpdateForSeconds(Config.disableCooldown));
        }
        
        private void ChangeButtonStatusToOff(AirlockButton button)
        {
            _enabledButtons.Remove(button);
            _disabledButtons.Add(button);
        }

        private void ChangeButtonStatusToOn(AirlockButton button)
        {
            _disabledButtons.Remove(button);
            _enabledButtons.Add(button);
        }
        
        private void InstantiateButtons()
        {
            buttonGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            buttonGrid.constraintCount = Config.gridSize;
            
            Vector2 cellSpacing = buttonGrid.spacing;
            Rect parentRect = buttonGrid.transform.GetComponent<RectTransform>().rect;
            RectOffset gridPadding = buttonGrid.padding;
            
            float targetCellWidth = (parentRect.width - gridPadding.horizontal) / Config.gridSize - cellSpacing.x;
            float targetCellHeight = (parentRect.height - gridPadding.vertical) / Config.gridSize - cellSpacing.y;

            buttonGrid.cellSize = new Vector2(targetCellWidth, targetCellHeight);

            for (int row = 0; row < Config.gridSize * Config.gridSize; row++)
            {
                var newButton = Instantiate(buttonPrefab, buttonGrid.transform);

                _allButtons.Add(newButton.GetComponent<AirlockButton>());
            }
        }
        
        protected override bool CheckWarningConditions()
        {
            return _disabledButtons.Count >= Config.warningThreshold;
        }

        protected override bool CheckFailConditions()
        {
            return _disabledButtons.Count >= Config.failThreshold;
        }
        
        protected override void HandleUpdateLogic()
        {
            StartCoroutine(DisableButtons());
        }
        
        private IEnumerator DisableButtons()
        {
            UpdateLogicIsPaused = true;
            
            int numberOfDisabledButtons =
                Random.Range(Config.minDisabledButtonsPerTurn, Config.maxDisabledButtonsPerTurn);
            
            for (int i = 0; i < numberOfDisabledButtons; i++)
            {
                var chosenButton = _enabledButtons[Random.Range(0, _enabledButtons.Count)];
                chosenButton.TurnOffSilently();
                _enabledButtons.Remove(chosenButton);
                _disabledButtons.Add(chosenButton);
            }

            yield return new WaitForSeconds(Config.disableCooldown);

            UpdateLogicIsPaused = false;
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
                yield return StartCoroutine(airlockButton.Blink(Config.singleBlinkTime, Config.totalBlinkCount));
            }
            
            _disabledButtons.Clear();
            _enabledButtons = new List<AirlockButton>(_allButtons);
        }
    }

    [Serializable]
    public class AirlockJamConfig : GeneralConfigBase
    {
        public int gridSize = 6;
        public int totalBlinkCount = 3;
        public float singleBlinkTime = 0.5f;
        public int minDisabledButtonsPerTurn = 2;
        public int maxDisabledButtonsPerTurn = 4;
        public float disableCooldown = 2f;
    }
}