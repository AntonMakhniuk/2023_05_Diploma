using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Production.Challenges.General.Rod_Positioning
{
    public class GenRodPositioning : GeneralBase<RodPositioningConfig>
    {
        [SerializeField] private List<RodLever> levers = new();
        
        // TODO: implement start method and instantiate levers

        protected override void HandleUpdateLogic()
        {
            foreach (var rodLever in levers)
            {
                rodLever.UpdateCurrentPosition();
            }
        }

        protected override bool CheckWarningConditions()
        {
            bool anyLeverIsInWarnRange = false;

            foreach (var rodLever in levers)
            {
                anyLeverIsInWarnRange = 
                    rodLever.PositionIsInWarningRange();
            }

            return anyLeverIsInWarnRange;
        }

        protected override bool CheckFailConditions()
        {
            bool anyLeverIsInFailRange = false;

            foreach (var rodLever in levers)
            {
                anyLeverIsInFailRange =
                    rodLever.PositionIsInDangerRange();
            }

            return anyLeverIsInFailRange;
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
            foreach (var rodLever in levers)
            {
                StartCoroutine(rodLever.ResetLever());
            }

            yield return null;
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
        [Range(0, 1)] public float warningRangeMinForSingle = 0.2f;
        [Range(0, 1)] public float warningRangeMaxForSingle = 0.4f;
        [Range(0, 1)] public float warningRangeMinForBoth = 0.1f;
        [Range(0, 1)] public float warningRangeMaxForBoth = 0.2f;
        [Range(0, 1)] public float maxStepLength = 0.001f;
        public int leverQuantity;
    }
}