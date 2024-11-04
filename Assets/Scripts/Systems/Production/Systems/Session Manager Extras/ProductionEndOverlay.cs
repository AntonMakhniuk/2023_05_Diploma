using System;
using Production.Crafting;
using TMPro;
using UnityEngine;

namespace Production.Systems.Session_Manager_Extras
{
    public class ProductionEndOverlay : MonoBehaviour
    {
        [SerializeField] private TMP_Text productionStatus, bonusChangeLog, finalBonusCount;
    
        private ProductionSessionManager _sessionManager;
        private CraftingData _craftingData;
        
        void Start()
        {
            _sessionManager = GetComponentInParent<ProductionSessionManager>();

            if (_sessionManager == null)
            {
                throw new Exception($"'{nameof(_sessionManager)}' is null. " +
                                    $"{GetType().Name} has been instantiated outside ProductionSessionManager");
            }

            _craftingData = _sessionManager.CraftingData;
            
            SetUpText();
        }
        
        private void SetUpText()
        {
            productionStatus.SetText(_craftingData.ProductionFailed 
                ? "<color=red>Failed</color>" : "<color=green>Succeeded</color>");

            bonusChangeLog.SetText($"> Baseline: {CraftingData.StartingBonusModifier}%\n");
            
            foreach (var bonusChange in _craftingData.BonusChangeLog)
            {
                string sourceType = "ERROR";
                
                switch (bonusChange.Key)
                {
                    case BonusSource.Morale:
                    {
                        sourceType = "Crew Morale";

                        break;
                    }
                    case BonusSource.GeneralFail:
                    {
                        sourceType = "Critical Fail";

                        break;
                    }
                    case BonusSource.ResourceSpecific:
                    {
                        sourceType = "Resource-Specific";

                        break;
                    }
                }
                
                bonusChangeLog.SetText(bonusChangeLog.text + $"> {sourceType}: {bonusChange.Value}%\n");
            }

            finalBonusCount.SetText($"{_craftingData.CurrentBonusModifier}%");
        }

        public void EndProduction()
        {
            _sessionManager.EndProduction();
        }
    }
}
