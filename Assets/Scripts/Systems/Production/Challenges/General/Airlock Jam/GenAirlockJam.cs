using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Systems.Production.Challenges.General;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Production.Challenges.General.Airlock_Jam
{
    public class GenAirlockJam : GeneralBase<AirlockJamConfig>
    {
        [SerializeField] private GridLayoutGroup buttonGrid;
        [SerializeField] private GameObject buttonPrefab;
        
        private readonly List<AirlockButton> _allButtons = new();

        public override void Setup()
        {
            base.Setup();
            
            InstantiateButtons();

            // Added so that buttons don't get disabled as soon as the challenge starts
            StartCoroutine(PauseUpdateForSeconds(Config.disableCooldown));
        }

        protected override void ChangeInteractive(bool newState)
        {
            foreach (var element in interactiveElementsParents)
            {
                element.SetActive(newState);
            }
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
        
        protected override int GetNumberOfWarnings()
        {
            return _allButtons.Count(b => b.isTurnedOn == false);
        }

        // As there is no middle state for buttons, in this challenge
        // the count of disabled ones goes for both warnings and fails
        protected override int GetNumberOfFails()
        {
            return _allButtons.Count(b => b.isTurnedOn == false);
        }

        protected override void UpdateChallengeElements()
        {
            StartCoroutine(DisableButtons());
        }
        
        private IEnumerator DisableButtons()
        {
            UpdateIsPaused = true;
            
            int numberOfDisabledButtons =
                Random.Range(Config.minDisabledButtonsPerTurn, Config.maxDisabledButtonsPerTurn);

            int numberOfEnabledButtons = _allButtons.Count(b => b.isTurnedOn);
            
            for (int i = 0; i < numberOfDisabledButtons; i++)
            {
                var enabledButtons = _allButtons.Where(b => b.isTurnedOn).ToList();
                var chosenButton = enabledButtons[Random.Range(0, numberOfEnabledButtons--)];
                chosenButton.TurnOffSilently();
            }

            yield return new WaitForSeconds(Config.disableCooldown);

            UpdateIsPaused = false;
        }
        
        protected override IEnumerator ResetLogicCoroutine()
        {
            foreach (var airlockButton in _allButtons)
            {
                StartCoroutine(airlockButton.Blink(Config.singleBlinkTime, Config.totalBlinkCount));
            }

            yield return new WaitForSeconds(Config.singleBlinkTime * Config.totalBlinkCount);
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