using System;
using TMPro;
using UnityEngine;

namespace Production.Systems.SessionManagerExtras
{
    public class ProductionSessionTimer : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        
        public ProductionSessionManager sessionManager;

        private float _currentTimeSeconds;
        private TimeSpan _currentTimeSpan;
        
        private void Start()
        {
            sessionManager = GetComponentInParent<ProductionSessionManager>();

            if (sessionManager == null)
            {
                throw new Exception($"'{nameof(sessionManager)}' is null. " +
                                    $"{GetType().Name} has been instantiated outside ProductionSessionManager");
            }

            _currentTimeSeconds = sessionManager.CraftingData.Recipe.difficultyConfig.productionLengthInSeconds;
        }

        private void FixedUpdate()
        {
            _currentTimeSeconds -= Time.fixedDeltaTime;

            if (_currentTimeSeconds <= 0)
            {
                OnTimerRanOut?.Invoke(this, null);
            }

            _currentTimeSpan = TimeSpan.FromSeconds(_currentTimeSeconds);
            timerText.text = _currentTimeSpan.ToString("mm\\:ss");
        }

        public event EventHandler OnTimerRanOut;
    }
}