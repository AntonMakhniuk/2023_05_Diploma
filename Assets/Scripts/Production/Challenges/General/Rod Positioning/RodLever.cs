using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Production.Challenges.General.Rod_Positioning
{
    public class RodLever : MonoBehaviour
    {
        private LeverDangerZoneType _dangerZoneType;
        private float _currentPosition;
        private float _safeRangeStart;
        private float _safeRangeEnd;
        private float _safeRangeCentre;
        private GenRodPositioning _parentPositioningChallenge;
        private RodPositioningConfig _config;

        private void Start()
        {
            _parentPositioningChallenge = GetComponentInParent<GenRodPositioning>();
            
            if (_parentPositioningChallenge == null)
            {
                throw new Exception("RodLever object has been instantiated outside of GenRodPositioning");
            }

            _config = _parentPositioningChallenge.Config;

            _dangerZoneType = Utility.GetRandomEnum<LeverDangerZoneType>();

            switch (_dangerZoneType)
            {
                case LeverDangerZoneType.BottomOnly:
                {
                    _safeRangeStart = _config.minRangeValue;
                    _safeRangeEnd = Random.Range(_config.minSafeRangeSize, _config.maxSafeRangeSize);
                    
                    break;
                }
                case LeverDangerZoneType.TopOnly:
                {
                    _safeRangeStart = _config.maxRangeValue - Random.Range(_config.minSafeRangeSize, _config.maxSafeRangeSize);
                    _safeRangeEnd = _config.maxRangeValue;

                    break;
                }
                case LeverDangerZoneType.Both:
                {
                    float safeRangeSize = Random.Range(_config.minSafeRangeSize, _config.maxSafeRangeSize);
                    float freeSpace = _config.maxRangeValue - _config.minDangerRangeSize * 2 
                                        - _config.failDistanceFromSafeRange * 2 
                                        - safeRangeSize;
                    
                    _safeRangeStart = _config.minDangerRangeSize + _config.failDistanceFromSafeRange 
                                                                 + Random.Range(0, freeSpace);
                    _safeRangeEnd = _safeRangeStart + safeRangeSize;
                    
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _safeRangeCentre = _safeRangeStart + (_safeRangeEnd - +_safeRangeStart) / 2;
            _currentPosition = Random.Range(_safeRangeStart, _safeRangeEnd);
        }
        
        private bool _isBeingReset;
        
        public void UpdateCurrentPosition()
        {
            if (_isBeingReset)
            {
                return;
            }
            
            switch (_dangerZoneType)
            {
                case LeverDangerZoneType.BottomOnly:
                {
                    _currentPosition += Random.Range(0, _config.maxStepLength);
                    
                    break;
                }
                case LeverDangerZoneType.TopOnly:
                {
                    _currentPosition -= Random.Range(0, _config.maxStepLength);

                    break;
                }
                case LeverDangerZoneType.Both:
                {
                    if (_currentPosition >= _safeRangeCentre)
                    {
                        _currentPosition += Random.Range(0, _config.maxStepLength);
                    }
                    else
                    {
                        _currentPosition -= Random.Range(0, _config.maxStepLength);
                    }

                    break;
                }
            }
        }

        private bool PositionIsInSafeRange()
        {
            return _safeRangeStart <= _currentPosition && _currentPosition <= _safeRangeEnd;
        }

        public float GetAbsoluteDistanceFromSafeRange()
        {
            if (PositionIsInSafeRange())
            {
                return 0f;
            }
            
            if (Math.Abs(_currentPosition - _safeRangeStart) <= Math.Abs(_currentPosition - _safeRangeEnd))
            {
                return Math.Abs(_currentPosition - _safeRangeStart);
            }

            return Math.Abs(_currentPosition - _safeRangeEnd);
        }

        public void ChangeCurrentPosition(float newPosition)
        {
            if (_isBeingReset)
            {
                return;
            }
            
            _currentPosition = newPosition;
        }

        public IEnumerator ResetLever()
        {
            _isBeingReset = true;
            
            float currentTime = 0f;
            float positionAtReset = _currentPosition;

            while (currentTime < _config.resetWaitingTime)
            {
                float t = currentTime / _config.resetWaitingTime;

                _currentPosition = Mathf.Lerp(positionAtReset, _safeRangeCentre, t);

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