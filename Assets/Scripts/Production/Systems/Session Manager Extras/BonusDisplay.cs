using System;
using TMPro;
using UnityEngine;

namespace Production.Systems.Session_Manager_Extras
{
    public class BonusDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text bonusText;
        
        private ProductionSessionManager _sessionManager;
        private float _currentBonus;
        
        private void Start()
        {
            _sessionManager = GetComponentInParent<ProductionSessionManager>();

            if (_sessionManager == null)
            {
                throw new Exception($"'{nameof(_sessionManager)}' is null. " +
                                    $"{GetType().Name} has been instantiated outside ProductionSessionManager");
            }

            _sessionManager.OnBonusChanged += AdjustBonusUsingModifier;
            
            AdjustBonusUsingModifier(_sessionManager.CraftingData.BonusModifier);
        }

        private void AdjustBonusUsingModifier(float bonusModifier)
        {
            _currentBonus += bonusModifier;

            bonusText.text = $"{_currentBonus}%";
        }
    }
}