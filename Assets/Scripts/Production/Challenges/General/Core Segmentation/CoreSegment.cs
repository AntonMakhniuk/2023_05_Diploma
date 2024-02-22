using System;
using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Production.Challenges.General.Core_Segmentation
{
    public class CoreSegment : MonoBehaviour
    {
        [SerializeField] private Transform edgePointTransform;
        
        private GenCoreSegmentation _segmentationChallenge;
        private Transform _transform;
        private Vector3 _basePosition;
        private SegmentState _currentState;
        private Coroutine _travelCoroutine;

        private void Start()
        {
            _segmentationChallenge = GetComponentInParent<GenCoreSegmentation>();

            if (_segmentationChallenge == null)
            {
                throw new Exception($"'{nameof(_segmentationChallenge)}' is null. " +
                                    $"{GetType().Name} has been instantiated outside GenSegmentationChallenge");
            }
            
            _transform = transform;
            _basePosition = _transform.position;
            _currentState = SegmentState.Stable;
            
            InvokeRepeating(nameof(HandleStateChecks), 0, 0.1f);
        }
        
        public event EventHandler<CoreSegment> OnSegmentEnteredFailZone;
        public event EventHandler<CoreSegment> OnSegmentEnteredWarningZone;
        public event EventHandler<CoreSegment> OnSegmentEnteredSafeZone;
        
        private void HandleStateChecks()
        {
            float distanceFromBase = Vector3.Distance(_basePosition, edgePointTransform.position);

            if (distanceFromBase >= _segmentationChallenge.failZoneRadiusScaled
                && _currentState != SegmentState.Unstable)
            {
                OnSegmentEnteredFailZone?.Invoke(this, this);

                _currentState = SegmentState.Unstable;
            }
            else if (distanceFromBase >= _segmentationChallenge.warningZoneRadiusScaled
                     && _currentState != SegmentState.Warning)
            {
                OnSegmentEnteredWarningZone?.Invoke(this, this);

                _currentState = SegmentState.Warning;
            }
            else if (distanceFromBase < _segmentationChallenge.warningZoneRadiusScaled
                     && _currentState != SegmentState.Stable)
            {
                OnSegmentEnteredSafeZone?.Invoke(this, this);
                
                _currentState = SegmentState.Stable;
            }
        }

        public IEnumerator MoveOutAndBackForReset()
        {
            InterruptAndMoveOut(_segmentationChallenge.Config.resetTravelTime);

            yield return new WaitForSeconds(_segmentationChallenge.Config.resetTravelTime);

            InterruptAndPutBack(_segmentationChallenge.Config.resetTravelTime);

            yield return new WaitForSeconds(_segmentationChallenge.Config.resetTravelTime);
        }
        
        public void InterruptAndMoveOut(float timeToTravel)
        {
            if (_travelCoroutine != null)
            {
                StopCoroutine(_travelCoroutine);
            }
            
            // 90f added to offset for the orientation of the prefab
            float angleInRadians = (_transform.rotation.eulerAngles.z + 90f
                                    - _segmentationChallenge.Config.numOfSegments / 2f)
                                   * Mathf.Deg2Rad;
            
            Vector3 direction = new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians), 0f);
            
            // Multiplied by 0.75f in order to make the pieces not fly all the way beyond the rim,
            // and by the localScale in order to account for the difference in aspect ratios
            Vector3 newPosition = _basePosition + direction * (_segmentationChallenge.failZoneRadiusScaled * 0.75f);
            
            _travelCoroutine = StartCoroutine(TravelCoroutine(newPosition, timeToTravel));
        }

        private void InterruptAndPutBack(float timeToTravel)
        {
            if (_travelCoroutine != null)
            {
                StopCoroutine(_travelCoroutine);
            }
            
            _travelCoroutine = StartCoroutine(TravelCoroutine(_basePosition, timeToTravel));
        }
        
        private IEnumerator TravelCoroutine(Vector3 newPosition, float timeToTravel)
        {
            float startTime = Time.time;
            float elapsedTime = 0f;
            Vector3 positionAtStart = _transform.position;
            
            while (elapsedTime < timeToTravel)
            {
                float t = elapsedTime / timeToTravel;

                _transform.position = Vector3.Lerp(positionAtStart, newPosition, t);

                elapsedTime = Time.time - startTime;

                yield return null;
            }
            
            _transform.position = newPosition;
        }
    }

    public enum SegmentState
    {
        Unstable, Warning, Stable
    }
}