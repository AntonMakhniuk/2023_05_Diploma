using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Production.Challenges.General.Rod_Positioning
{
    public class RodLever : MonoBehaviour
    {
        private float _safeRangeCentre;
        private GenRodPositioning _parentPositioningChallenge;
        
        public LeverDangerZoneType dangerZoneType;
        public float safeRangeStart;
        public float safeRangeEnd;
        public float dangerZoneSingleStart;
        public float dangerZoneBothStart;
        public float currentPosition;
        public RodPositioningConfig config;

        private void Start()
        {
            _parentPositioningChallenge = GetComponentInParent<GenRodPositioning>();
            
            if (_parentPositioningChallenge == null)
            {
                throw new Exception("RodLever object has been instantiated outside of GenRodPositioning");
            }

            config = _parentPositioningChallenge.Config;

            dangerZoneType = Utility.GetRandomEnum<LeverDangerZoneType>();

            switch (dangerZoneType)
            {
                case LeverDangerZoneType.TopOnly:
                {
                    safeRangeStart = config.minRangeValue;
                    safeRangeEnd = Random.Range(config.minSafeRangeSize, config.maxSafeRangeSize);

                    dangerZoneSingleStart = safeRangeEnd +
                                             Random.Range(config.warningRangeMinForSingle,
                                                 config.warningRangeMaxForSingle);
                    
                    break;
                }
                case LeverDangerZoneType.BottomOnly:
                {
                    safeRangeStart = config.maxRangeValue - Random.Range(config.minSafeRangeSize, config.maxSafeRangeSize);
                    safeRangeEnd = config.maxRangeValue;
                    
                    dangerZoneSingleStart = safeRangeStart -
                                             Random.Range(config.warningRangeMinForSingle,
                                                 config.warningRangeMaxForSingle);

                    break;
                }
                case LeverDangerZoneType.Both:
                {
                    float safeRangeSize = Random.Range(config.minSafeRangeSize, config.maxSafeRangeSize);
                    float topWarningRange = 
                        Random.Range(config.warningRangeMinForBoth, config.warningRangeMaxForBoth);
                    float bottomWarningRange = 
                        Random.Range(config.warningRangeMinForBoth, config.warningRangeMaxForBoth);
                    float totalNonDangerRange = bottomWarningRange + safeRangeSize + topWarningRange;
                    float totalFreeDangerRange = 1f - totalNonDangerRange - 2 * config.minDangerRangeSize;
                    float dangerRangeSplit = Random.Range(0, totalFreeDangerRange);
                    float bottomDangerSize = config.minDangerRangeSize + dangerRangeSplit;
                    float topDangerSize = config.minDangerRangeSize + totalFreeDangerRange - dangerRangeSplit;

                    dangerZoneSingleStart = bottomDangerSize;
                    safeRangeStart = dangerZoneSingleStart + bottomWarningRange;
                    safeRangeEnd = safeRangeStart + safeRangeSize;
                    dangerZoneBothStart = 1f - topDangerSize;
                    
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _safeRangeCentre = safeRangeStart + (safeRangeEnd - +safeRangeStart) / 2;
            currentPosition = Random.Range(safeRangeStart, safeRangeEnd);
        }
        
        private bool _isBeingReset;
        
        public void UpdateCurrentPosition()
        {
            if (_isBeingReset)
            {
                return;
            }
            
            switch (dangerZoneType)
            {
                case LeverDangerZoneType.BottomOnly:
                {
                    currentPosition -= Random.Range(0, config.maxStepLength);
                    
                    break;
                }
                case LeverDangerZoneType.TopOnly:
                {
                    currentPosition += Random.Range(0, config.maxStepLength);

                    break;
                }
                case LeverDangerZoneType.Both:
                {
                    if (currentPosition >= _safeRangeCentre)
                    {
                        currentPosition += Random.Range(0, config.maxStepLength);
                    }
                    else
                    {
                        currentPosition -= Random.Range(0, config.maxStepLength);
                    }

                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        private bool PositionIsInSafeRange()
        {
            return safeRangeStart <= currentPosition && currentPosition <= safeRangeEnd;
        }

        public bool PositionIsInWarningRange()
        {
            switch (dangerZoneType)
            {
                case LeverDangerZoneType.TopOnly:
                {
                    return currentPosition > safeRangeEnd && currentPosition < dangerZoneSingleStart;
                }
                case LeverDangerZoneType.BottomOnly:
                {
                    return currentPosition < safeRangeStart && currentPosition > dangerZoneSingleStart;
                }
                case LeverDangerZoneType.Both:
                {
                    return (currentPosition > dangerZoneSingleStart && currentPosition < safeRangeStart) 
                        || (currentPosition > safeRangeEnd && currentPosition < dangerZoneBothStart);
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool PositionIsInDangerRange()
        {
            switch (dangerZoneType)
            {
                case LeverDangerZoneType.TopOnly:
                {
                    return currentPosition > dangerZoneSingleStart;
                }
                case LeverDangerZoneType.BottomOnly:
                {
                    return currentPosition < dangerZoneSingleStart;
                }
                case LeverDangerZoneType.Both:
                {
                    return currentPosition < dangerZoneSingleStart || currentPosition > dangerZoneBothStart;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        public float GetAbsoluteDistanceFromSafeRange()
        {
            if (PositionIsInSafeRange())
            {
                return 0f;
            }
            
            if (Math.Abs(currentPosition - safeRangeStart) <= Math.Abs(currentPosition - safeRangeEnd))
            {
                return Math.Abs(currentPosition - safeRangeStart);
            }

            return Math.Abs(currentPosition - safeRangeEnd);
        }

        public void ChangeCurrentPosition(float newPosition)
        {
            if (_isBeingReset)
            {
                return;
            }
            
            currentPosition = newPosition;
        }

        public IEnumerator ResetLever()
        {
            _isBeingReset = true;
            
            float currentTime = 0f;
            float positionAtReset = currentPosition;

            while (currentTime < config.resetWaitingTime)
            {
                float t = currentTime / config.resetWaitingTime;

                currentPosition = Mathf.Lerp(positionAtReset, _safeRangeCentre, t);

                currentTime += Time.deltaTime;
            }

            _isBeingReset = false;
            
            yield return null;
        }
    }

    public enum LeverDangerZoneType
    {
        BottomOnly, TopOnly, Both
    }
}