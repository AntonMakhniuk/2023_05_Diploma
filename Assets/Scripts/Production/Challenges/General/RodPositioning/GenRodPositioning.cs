using System;
using System.Collections;
using UnityEngine;

namespace Production.Challenges.General.RodPositioning
{
    public class GenRodPositioning : GeneralBase<RodPositioningConfig>
    {
        private RodLever[] _levers;
        
        // TODO: implement start method and instantiate levers
        
        protected override void HandleUpdateLogic()
        {
            foreach (var rodLever in _levers)
            {
                rodLever.UpdateCurrentPosition();
            }
        }

        protected override bool CheckWarningConditions()
        {
            bool anyLeverIsInFailRange = false;

            foreach (var rodLever in _levers)
            {
                anyLeverIsInFailRange = 
                    rodLever.GetAbsoluteDistanceFromSafeRange() >= Config.failDistanceFromSafeRange;
            }

            return anyLeverIsInFailRange;
        }

        protected override bool CheckFailConditions()
        {
            bool anyLeverIsInWarnRange = false;

            foreach (var rodLever in _levers)
            {
                anyLeverIsInWarnRange = 
                    rodLever.GetAbsoluteDistanceFromSafeRange() >= Config.warningDistanceFromSafeRange;
            }

            return anyLeverIsInWarnRange;
        }
        
        public event EventHandler OnRodPositioningAboveWarningThreshold;
        
        protected override void HandleWarningStart()
        {
            OnRodPositioningAboveWarningThreshold?.Invoke(this, null);
        }

        public event EventHandler OnRodPositioningBelowWarningThreshold;
        
        protected override void HandleWarningStop()
        {
            OnRodPositioningBelowWarningThreshold?.Invoke(this, null);
        }

        protected override IEnumerator HandleResetLogic()
        {
            foreach (var rodLever in _levers)
            {
                StartCoroutine(rodLever.ResetLever());

                yield return null;
            }
        }
    }

    [Serializable]
    public class RodPositioningConfig : GeneralConfigBase
    {
        public float maxRangeValue = 1f;
        public float minRangeValue;
        [Range(0, 1)] public float minSafeRangeSize = 0.15f;
        [Range(0, 1)] public float maxSafeRangeSize = 0.2f;
        [Range(0, 1)] public float minDangerRangeSize = 0.1f;
        [Range(0, 1)] public float warningDistanceFromSafeRange = 0f;
        [Range(0, 1)] public float failDistanceFromSafeRange = 0.2f;
        [Range(0, 1)] public float maxStepLength = 0.001f;
    }
}