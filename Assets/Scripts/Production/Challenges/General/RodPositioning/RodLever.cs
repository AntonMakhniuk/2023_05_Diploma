using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Production.Challenges.General.RodPositioning
{
    public class RodLever : MonoBehaviour
    {
        private LeverDangerZoneType _dangerZoneType;
        [Range(0, 1)] private float _currentPosition;
        [Range(0, 1)] private float _safeRangeStart;
        [Range(0, 1)] private float _safeRangeEnd;
        private float _safeRangeCentre;
        private bool _isBeingReset;
        // TODO: instantiation with passing of the config
        private RodPositioningConfig _config;

        private void Start()
        {
            _dangerZoneType = Utility.GetRandomEnum<LeverDangerZoneType>();

            switch (_dangerZoneType)
            {
                case LeverDangerZoneType.BottomOnly:
                {
                    _safeRangeStart = 0;
                    _safeRangeEnd = Random.Range(_config.minSafeRangeSize, _config.maxSafeRangeSize);
                    
                    break;
                }
                case LeverDangerZoneType.TopOnly:
                {
                    _safeRangeStart = 1 - Random.Range(_config.minSafeRangeSize, _config.maxSafeRangeSize);
                    _safeRangeEnd = 1;

                    break;
                }
                case LeverDangerZoneType.Both:
                {
                    float safeRangeSize = Random.Range(_config.minSafeRangeSize, _config.maxSafeRangeSize);
                    float freeSpace = 1 - _config.minDangerRangeSize * 2 
                                        - _config.failDistanceFromSafeRange * 2 
                                        - safeRangeSize;
                    
                    _safeRangeStart = _config.minDangerRangeSize + _config.failDistanceFromSafeRange 
                                                                 + Random.Range(0, freeSpace);
                    _safeRangeEnd = _safeRangeStart + safeRangeSize;
                    
                    break;
                }
            }

            _safeRangeCentre = _safeRangeStart + (_safeRangeEnd - +_safeRangeStart) / 2;
            _currentPosition = Random.Range(_safeRangeStart, _safeRangeEnd);
        }

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
        
        public bool PositionIsInSafeRange()
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

            while (currentTime < _config.failureResetTime)
            {
                float t = currentTime / _config.failureResetTime;

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