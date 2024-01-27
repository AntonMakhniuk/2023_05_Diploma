using System;
using UnityEngine;

namespace Production.Systems.SessionManagerExtras
{
    public class TimerSessionManagerCommunicator : MonoBehaviour
    {
        public ProductionSessionTimer sessionTimer;
        
        private ProductionSessionManager _sessionManager;

        private void Start()
        {
            _sessionManager = sessionTimer.sessionManager;

            sessionTimer.OnTimerRanOut += StopProductionInManager;
        }

        private void OnDestroy()
        {
            sessionTimer.OnTimerRanOut -= StopProductionInManager;
        }

        private void StopProductionInManager(object sender, EventArgs eventArgs)
        {
            _sessionManager.FinishProduction();
        }
    }
}