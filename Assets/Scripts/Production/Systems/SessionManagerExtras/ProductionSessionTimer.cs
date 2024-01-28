using System;
using TMPro;
using UnityEngine;

namespace Production.Systems.SessionManagerExtras
{
    public class ProductionSessionTimer : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        
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
        }

        private void FixedUpdate()
        {
            _currentTimeSeconds -= Time.fixedDeltaTime;

            if (_currentTimeSeconds <= 0)
            {
                StopProductionInManager();
            }

            _currentTimeSpan = TimeSpan.FromSeconds(_currentTimeSeconds);
            timerText.text = _currentTimeSpan.ToString("mm\\:ss");
        }
        
        private void StopProductionInManager()
        {
            _sessionManager.FinishProduction();
        }
    }
}