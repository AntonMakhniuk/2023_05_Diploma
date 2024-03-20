using System;
using System.Collections;
using Miscellaneous;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Production.Challenges.General.Rod_Positioning
{
    public class RodLever : MonoBehaviour
    {
        private float _safeRangeCentre;
        private GenRodPositioning _parentPositioningChallenge;
        private RodPositioningConfig _config;
        
        [HideInInspector] public LeverDangerZoneType dangerZoneType;
        [HideInInspector] public float safeRangeStart;
        [HideInInspector] public float safeRangeEnd;
        [HideInInspector] public float dangerZoneSingleStart;
        [HideInInspector] public float dangerZoneBothStart;
        [HideInInspector] public float currentPosition;
        [HideInInspector] public bool isInteractable;

        private void Start()
        {
            _parentPositioningChallenge = GetComponentInParent<GenRodPositioning>();
            
            if (_parentPositioningChallenge == null)
            {
                throw new Exception("RodLever object has been instantiated outside of GenRodPositioning");
            }

            if (!_parentPositioningChallenge.isActive)
            {
                return;
            }
                
            _config = _parentPositioningChallenge.Config;

            dangerZoneType = Utility.GetRandomEnum<LeverDangerZoneType>();

            switch (dangerZoneType)
            {
                case LeverDangerZoneType.TopOnly:
                {
                    safeRangeStart = _config.minRangeValue;
                    safeRangeEnd = Random.Range(_config.minSafeRangeSize, _config.maxSafeRangeSize);

                    dangerZoneSingleStart = safeRangeEnd +
                                             Random.Range(_config.warningRangeMinForSingle,
                                                 _config.warningRangeMaxForSingle);
                    
                    break;
                }
                case LeverDangerZoneType.BottomOnly:
                {
                    safeRangeStart = _config.maxRangeValue - Random.Range(_config.minSafeRangeSize, _config.maxSafeRangeSize);
                    safeRangeEnd = _config.maxRangeValue;
                    
                    dangerZoneSingleStart = safeRangeStart -
                                             Random.Range(_config.warningRangeMinForSingle,
                                                 _config.warningRangeMaxForSingle);

                    break;
                }
                case LeverDangerZoneType.Both:
                {
                    float safeRangeSize = Random.Range(_config.minSafeRangeSize, _config.maxSafeRangeSize);
                    float topWarningRange = 
                        Random.Range(_config.warningRangeMinForBoth, _config.warningRangeMaxForBoth);
                    float bottomWarningRange = 
                        Random.Range(_config.warningRangeMinForBoth, _config.warningRangeMaxForBoth);
                    float totalNonDangerRange = bottomWarningRange + safeRangeSize + topWarningRange;
                    float totalFreeDangerRange = 1f - totalNonDangerRange - 2 * _config.minDangerRangeSize;
                    float dangerRangeSplit = Random.Range(0, totalFreeDangerRange);
                    float bottomDangerSize = _config.minDangerRangeSize + dangerRangeSplit;
                    float topDangerSize = _config.minDangerRangeSize + totalFreeDangerRange - dangerRangeSplit;

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
            isInteractable = true;
        }
        
        public void UpdateCurrentPosition()
        {
            if (!isInteractable)
            {
                return;
            }
            
            switch (dangerZoneType)
            {
                case LeverDangerZoneType.BottomOnly:
                {
                    currentPosition -= Random.Range(0, _config.maxStepLength);
                    
                    break;
                }
                case LeverDangerZoneType.TopOnly:
                {
                    currentPosition += Random.Range(0, _config.maxStepLength);

                    break;
                }
                case LeverDangerZoneType.Both:
                {
                    if (currentPosition >= _safeRangeCentre)
                    {
                        currentPosition += Random.Range(0, _config.maxStepLength);
                    }
                    else
                    {
                        currentPosition -= Random.Range(0, _config.maxStepLength);
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
            if (!isInteractable)
            {
                return;
            }
            
            currentPosition = newPosition;
        }

        public IEnumerator ResetLever()
        {
            isInteractable = false;
            
            float startTime = Time.time;
            float elapsedTime = 0f;
            float positionAtReset = currentPosition;

            while (elapsedTime < _config.resetWaitingTime)
            {
                float t = elapsedTime / _config.resetWaitingTime;

                currentPosition = Mathf.Lerp(positionAtReset, _safeRangeCentre, t);

                elapsedTime = Time.time - startTime;
                
                yield return null;
            }

            currentPosition = _safeRangeCentre;
            
            isInteractable = true;
        }
    }

    public enum LeverDangerZoneType
    {
        BottomOnly, TopOnly, Both
    }
}