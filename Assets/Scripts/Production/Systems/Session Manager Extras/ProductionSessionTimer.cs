using System;
using TMPro;
using UnityEngine;

namespace Production.Systems.Session_Manager_Extras
{
    public class ProductionSessionTimer : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private float updateRate = 0.0333f;
        
        private ProductionSessionManager _sessionManager;
        private float _currentTimeSeconds;
        private TimeSpan _currentTimeSpan;
        
        private void Start()
        {
            _sessionManager = GetComponentInParent<ProductionSessionManager>();

            if (_sessionManager == null)
            {
                throw new Exception($"'{nameof(_sessionManager)}' is null. " +
                                    $"{GetType().Name} has been instantiated outside ProductionSessionManager");
            }

            _currentTimeSeconds = _sessionManager.CraftingData.Recipe.difficultyConfig.productionLengthInSeconds;
            
            InvokeRepeating(nameof(UpdateTimer), 0, updateRate);
        }

        private void UpdateTimer()
        {
            _currentTimeSeconds -= updateRate;

            if (_currentTimeSeconds <= 0)
            {
                StopProductionInManager();
            }

            _currentTimeSpan = TimeSpan.FromSeconds(_currentTimeSeconds);
            timerText.text = _currentTimeSpan.ToString("mm\\:ss");
        }
        
        private void StopProductionInManager()
        {
            _sessionManager.FinishProductionSuccessfully();
        }
    }
}