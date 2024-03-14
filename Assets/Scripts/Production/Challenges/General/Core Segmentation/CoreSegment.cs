using System;
using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Production.Challenges.General.Core_Segmentation
{
    public class CoreSegment : MonoBehaviour
    {
        private SegmentState _currentState;
        
        public Transform edgePointTransform;
        
        private GenCoreSegmentation _segmentationChallenge;
        private Transform _transform;
        private Vector3 _centerPosition;
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
            _centerPosition = _transform.position;
            _currentState = SegmentState.Stable;
            
            InvokeRepeating(nameof(HandleStateChecks), 0, _segmentationChallenge.updateRate);
        }

        public event EventHandler<CoreSegment> OnSegmentEnteredFailZone,
            OnSegmentEnteredWarningZone,
            OnSegmentLeftCenter,
            OnSegmentEnteredCenter;
        
        private void HandleStateChecks()
        {
            float distanceFromBase = Vector3.Distance(_centerPosition, edgePointTransform.position);

            if (_currentState != SegmentState.Unstable
                && distanceFromBase >= _segmentationChallenge.failZoneRadiusScaled)
            {
                OnSegmentEnteredFailZone?.Invoke(this, this);

                _currentState = SegmentState.Unstable;
                
                Debug.Log("Unstable");
            }
            else if (_currentState != SegmentState.Warning
                     && distanceFromBase < _segmentationChallenge.failZoneRadiusScaled
                     && distanceFromBase >= _segmentationChallenge.warningZoneRadiusScaled)
            {
                OnSegmentEnteredWarningZone?.Invoke(this, this);

                _currentState = SegmentState.Warning;
                
                Debug.Log("Warning");
            }
            else if (_currentState != SegmentState.LeavingCenter
                     && distanceFromBase < _segmentationChallenge.warningZoneRadiusScaled
                     && transform.position != _centerPosition)
            {
                OnSegmentLeftCenter?.Invoke(this, this);
                
                _currentState = SegmentState.LeavingCenter;
                
                Debug.Log("Leaving Center");
            }
            else if (_currentState != SegmentState.Stable
                     && transform.position == _centerPosition)
            {
                OnSegmentEnteredCenter?.Invoke(this, this);
                
                _currentState = SegmentState.Stable;
                
                Debug.Log("Stable");
            }
        }

        public IEnumerator MoveOutAndBack()
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
            
            Vector3 newPosition = _centerPosition + direction 
                * (_segmentationChallenge.failZoneRadiusScaled 
                   - Vector3.Distance(edgePointTransform.position, _transform.position) * 0.8f);
            
            _travelCoroutine = StartCoroutine(TravelCoroutine(newPosition, timeToTravel));
        }

        public void InterruptAndPutBack(float timeToTravel)
        {
            if (_travelCoroutine != null)
            {
                StopCoroutine(_travelCoroutine);
            }
            
            _travelCoroutine = StartCoroutine(TravelCoroutine(_centerPosition, timeToTravel));
        }
        
        
        // TODO: use utility version
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
        Unstable, Warning, Stable, LeavingCenter
    }
}