using System;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Production.Systems.SessionManagerExtras
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
        }

        private void ActivateNextErrorDisplay(object sender, int criticalFailCount)
        {
            if (criticalFailCount > errorDisplays.Length)
            {
                Debug.LogError("More critical errors sent than there are available error displays.");
                
                return;
            }
            
            // the subtraction accounts for the indexing starting from 0 and not 1
            var imageToChange = errorDisplays[criticalFailCount - 1].GetComponent<Image>();

            Color.RGBToHSV(imageToChange.color, out var h, out var s, out var _);
            
            imageToChange.color = Color.HSVToRGB(h, s, 1);
        }
    }
}