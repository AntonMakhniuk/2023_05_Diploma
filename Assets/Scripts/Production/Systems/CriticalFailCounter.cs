using System;
using UnityEngine;

namespace Production.Systems
{
    public class CriticalFailCounter : MonoBehaviour
    {
        [SerializeField] private GameObject[] errorDisplays;

        private ProductionSessionManager _sessionManager;

        private void Start()
        {
            _sessionManager = GetComponentInParent<ProductionSessionManager>();

            if (_sessionManager == null)
            {
                throw new Exception($"'{nameof(_sessionManager)}' is null. " +
                                    $"{GetType().Name} has been instantiated outside ProductionSessionManager");
            }

            _sessionManager.OnCriticalFailReachedManager += ActivateNextErrorDisplay;

            foreach (var display in errorDisplays)
            {
                display.SetActive(false);
            }
        }

        private void ActivateNextErrorDisplay(object sender, int criticalFailCount)
        {
            if (criticalFailCount > errorDisplays.Length)
            {
                Debug.Log("More critical errors sent than there are available error displays.");
                
                return;
            }
            
            // the subtraction accounts for the indexing starting from 0 and not 1
            errorDisplays[criticalFailCount - 1].SetActive(true);
        }
    }
}