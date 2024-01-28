using System;
using System.Collections;
using Production.Systems;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Production.Challenges.General.Core_Segmentation
{
    public class CoreSegment : MonoBehaviour
    {
        private Vector3 _basePosition;
        private bool _isStable = true;
        private bool _isInteractable = true;
        private GenCoreSegmentation _segmentationChallenge;
        private Transform _objectTransform;
        private Quaternion _objectRotation;
        private float _lengthOfSide;

        private void Start()
        {
            _segmentationChallenge = GetComponentInParent<GenCoreSegmentation>();

            if (_segmentationChallenge == null)
            {
                throw new Exception($"'{nameof(_segmentationChallenge)}' is null. " +
                                    $"{GetType().Name} has been instantiated outside GenSegmentationChallenge");
            }

            _objectTransform = transform;
            _objectRotation = _objectTransform.rotation;
            _basePosition = _objectTransform.position;
            
            // TODO: Remove this ugly behavior
            
            _lengthOfSide = GetComponent<RectTransform>().rect.width * _objectTransform.localScale.x;
        }

        public IEnumerator MoveOutAndBackForReset()
        {
            _isInteractable = false;
            
            MoveOut(_segmentationChallenge.Config.resetTravelTime);

            yield return new WaitForSeconds(_segmentationChallenge.Config.resetTravelTime);

            PutBack(_segmentationChallenge.Config.resetTravelTime);

            yield return new WaitForSeconds(_segmentationChallenge.Config.resetTravelTime);
            
            _isInteractable = true;

            yield return null;
        }
        
        public delegate void SegmentStatusHandler(CoreSegment segment);
        public event SegmentStatusHandler OnSegmentInFailZone;
        
        public void MoveOut(float timeToTravel)
        {
            if (!_isStable || !_isInteractable)
            {
                return;
            }
            float angleInDegrees = _objectRotation.eulerAngles.z;
            
            // Normalizing and adding an extra half segment degrees so that it moves in the center of the segment
            angleInDegrees = (angleInDegrees + 360f) % 360f;
            angleInDegrees -= 360f / _segmentationChallenge.Config.numOfSegments / 2;
            
            // TODO: fix weird bug with some segments floating out further than others
            
            float angleInRadians = Mathf.Deg2Rad * angleInDegrees;
            float failColliderRadius = _segmentationChallenge.failCollider.segmentationCircleCollider.radius;

            // Scaling by the Production Manager as it is the canvas and as such its scale
            // will directly affect how far this position would end up
            Vector3 newPosition = _objectTransform.position 
                                  + new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians), 0f) 
                                  * ((failColliderRadius - _lengthOfSide * 0.6f) * ProductionManager.Instance.transform.localScale.x);
            
            StartCoroutine(TravelCoroutine(newPosition, timeToTravel));
        }

        public event SegmentStatusHandler OnSegmentStabilized;

        public void PutBack(float timeToTravel)
        {
            if (_isStable || !_isInteractable)
            {
                return;
            }

            StartCoroutine(TravelCoroutine(_basePosition, timeToTravel));
        }
        
        private IEnumerator TravelCoroutine(Vector3 newPosition, float timeToTravel)
        {
            float startTime = Time.time;
            float elapsedTime = 0f;
            Vector3 positionAtPutBackStart = _objectTransform.position;
            
            while (elapsedTime < timeToTravel)
            {
                float t = elapsedTime / timeToTravel;

                _objectTransform.position = Vector3.Lerp(positionAtPutBackStart, newPosition, t);

                elapsedTime = Time.time - startTime;

                yield return null;
            }
            
            _objectTransform.position = newPosition;
            _isStable = !_isStable;

            yield return null;
        }

        public event SegmentStatusHandler OnSegmentInWarningZone;
        public event SegmentStatusHandler OnSegmentInSafeZone;
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other == _segmentationChallenge.warningCollider.segmentationCircleCollider)
            {
                OnSegmentInWarningZone?.Invoke(this);
            }

            if (other == _segmentationChallenge.failCollider.segmentationCircleCollider)
            {
                OnSegmentInFailZone?.Invoke(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == _segmentationChallenge.warningCollider.segmentationCircleCollider)
            {
                OnSegmentInSafeZone?.Invoke(this);
            }

            if (other == _segmentationChallenge.failCollider.segmentationCircleCollider)
            {
                OnSegmentStabilized?.Invoke(this);
            }
        }
    }
}