using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Production.Challenges.General.RodPositioning
{
    public class GenRodPositioning : GeneralBase<RodPositioningConfig>
    {
        private RodLever[] _levers;
        private bool _isBeingReset;
        private bool _isWarning;
        
        public delegate void RodPositioningWarningHandler();
        public event RodPositioningWarningHandler RodPositioningWarningSurpassed;
        
        protected override void UpdateChallenge()
        {
            if (_isBeingReset)
            {
                return;
            }
            
            foreach (var rodLever in _levers)
            {
                rodLever.UpdateCurrentPosition();
                
                if (rodLever.PositionIsInSafeRange())
                {
                    continue;
                }
                
                if (rodLever.GetAbsoluteDistanceFromSafeRange() >= Config.failDistanceFromSafeRange)
                {
                    Fail();
                }
                else if (rodLever.GetAbsoluteDistanceFromSafeRange() >= Config.warningDistanceFromSafeRange)
                {
                    if (_isWarning)
                    {
                        continue;
                    }

                    StartCoroutine(Warn());
                }
            }
        }

        private IEnumerator Warn()
        {
            _isWarning = true;
            
            RodPositioningWarningSurpassed?.Invoke();

            yield return new WaitForSeconds(Config.warningTime);

            _isWarning = false;
            
            yield return null;
        }

        protected override void Reset()
        {
            if (_isBeingReset)
            {
                return;
            }

            StartCoroutine(ResetLevers());
        }

        private IEnumerator ResetLevers()
        {
            _isBeingReset = true;

            foreach (var rodLever in _levers)
            {
                StartCoroutine(rodLever.ResetLever());
            }

            yield return new WaitForSeconds(Config.failureResetTime);
            
            _isBeingReset = false;

            yield return null;
        }
    }

    [Serializable]
    public class RodPositioningConfig : ConfigBase
    {
        [Range(0, 1)] public float minSafeRangeSize = 0.15f;
        [Range(0, 1)] public float maxSafeRangeSize = 0.2f;
        [Range(0, 1)] public float minDangerRangeSize = 0.1f;
        [Range(0, 1)] public float warningDistanceFromSafeRange = 0f;
        [Range(0, 1)] public float failDistanceFromSafeRange = 0.2f;
        [Range(0, 1)] public float maxStepLength = 0.001f;
        public float warningTime = 1f;
        public float failureResetTime = 1f;
    }
}