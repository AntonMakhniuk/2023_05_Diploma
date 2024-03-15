using System;
using System.Collections;
using Miscellaneous;
using Production.Systems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Production.Challenges.General.Core_Segmentation
{
    public class Stabilizer : MonoBehaviour
    {
        public Transform edgePointTransform;
        
        [SerializeField] private float baseOrbitMovementSpeed;
        [SerializeField] private float maxStabilizationTime;
        [SerializeField] private float resetSpeedUpModifier;
        [SerializeField] private bool clockwiseRotation = true;
        
        private GenCoreSegmentation _segmentationChallenge;
        private Transform _transform;
        private Vector3 _position;
        private Vector3 _centerPosition;
        private float _stabilizerOrbitRadiusScaled;
        private float _currentMovementSpeed;
        private float _currentAngle;
        
        private void Start()
        {
            _segmentationChallenge = GetComponentInParent<GenCoreSegmentation>();

            if (_segmentationChallenge == null)
            {
                throw new Exception($"'{nameof(_segmentationChallenge)}' is null. " +
                                    $"{GetType().Name} has been instantiated outside GenSegmentationChallenge");
            }
            
            _transform = transform;
            _position = _transform.position;
            _centerPosition = _segmentationChallenge.childCanvas.transform.position;
            _stabilizerOrbitRadiusScaled = _segmentationChallenge.stabilizerOrbitRadius 
                                     * ProductionManager.Instance.transform.localScale.x;
            _currentAngle = Random.Range(0, 360);
            
            UpdatePosition();
            
            InvokeRepeating(nameof(UpdatePosition), 0, _segmentationChallenge.updateRate);
            InvokeRepeating(nameof(CheckOverlap), 0, _segmentationChallenge.updateRate);
            StartCoroutine(SpeedUpAtStart());
        }
        
        private IEnumerator SpeedUpAtStart()
        {
            IEnumerator currentEnumerator = Utility.LerpFloat
            (
                0,
                baseOrbitMovementSpeed,
                AnimationCurve.Linear(0, 0, 1, 1), 
                _segmentationChallenge.Config.resetWaitingTime
            );
            
            while (currentEnumerator.MoveNext())
            {
                _currentMovementSpeed = (float) currentEnumerator.Current!;

                yield return null;
            }
        }
        
        [HideInInspector] public bool rotationMovementIsPaused;
        
        private void UpdatePosition()
        {
            if (rotationMovementIsPaused)
            {
                return;
            }

            int invertDirection = clockwiseRotation ? -1 : 1;
            
            // Multiplied by -1 to invert the movement direction
            _currentAngle += _currentMovementSpeed * _segmentationChallenge.updateRate * invertDirection;
            float newX = _centerPosition.x + Mathf.Cos(_currentAngle) * _stabilizerOrbitRadiusScaled;
            float newY = _centerPosition.y + Mathf.Sin(_currentAngle) * _stabilizerOrbitRadiusScaled;
            
            transform.position = new Vector3(newX, newY, _position.z);
            
            float angleToCenter =
                Mathf.Atan2(_centerPosition.y - newY, _centerPosition.x - newX) * Mathf.Rad2Deg;
            
            _transform.rotation = Quaternion.Euler(0f, 0f, angleToCenter);
        }

        private bool _isPointingAtSegment;

        public delegate void TrajectoryUpdateHandler(RaycastHit2D hit, bool isPointingAtSegment);
        public event TrajectoryUpdateHandler OnStabilizerTrajectoryUpdated;
        
        private void CheckOverlap()
        {
            var position = edgePointTransform.position;
            
            Debug.DrawLine(position, _centerPosition);

            RaycastHit2D hit = Physics2D.Linecast(position,_centerPosition);
            
            _isPointingAtSegment = hit.transform.gameObject.TryGetComponent<CoreSegment>(out _);
            
            OnStabilizerTrajectoryUpdated?.Invoke(hit, _isPointingAtSegment);
        }
        
        [SerializeField] private AnimationCurve speedUpCurve, slowDownCurve, slowSpeedUpCurve;

        private bool _isResetting;
        
        [Header("Use if there is a StabilizerLineRenderer attached; otherwise set to 0")]
        public float rayMovementTime;
        
        public IEnumerator SpeedUpAndReset(float resetTime)
        {
            _isResetting = true;
            
            float topMovementSpeed = baseOrbitMovementSpeed * resetSpeedUpModifier;
            
            IEnumerator currentEnumerator = Utility.LerpFloat
            (
                baseOrbitMovementSpeed,
                topMovementSpeed,
                speedUpCurve,
                resetTime * 0.5f
            );
            
            while (currentEnumerator.MoveNext())
            {
                _currentMovementSpeed = (float) currentEnumerator.Current!;

                yield return null;
            }
            
            currentEnumerator = Utility.LerpFloat
            (
                topMovementSpeed,
                0,
                slowDownCurve,
                resetTime * 0.5f
            );
            
            while (currentEnumerator.MoveNext())
            {
                _currentMovementSpeed = (float) currentEnumerator.Current!;

                yield return null;
            }

            if (ProductionManager.Instance.currentManager.productionIsOver)
            {
                CancelInvoke();

                yield break;
            }
            
            StartCoroutine(InvertResetStateAfterTime(rayMovementTime));
            
            currentEnumerator = Utility.LerpFloat
            (
                0,
                baseOrbitMovementSpeed,
                slowSpeedUpCurve, 
                resetTime
            );
            
            while (currentEnumerator.MoveNext())
            {
                _currentMovementSpeed = (float) currentEnumerator.Current!;

                yield return null;
            }
        }

        private IEnumerator InvertResetStateAfterTime(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);

            _isResetting = !_isResetting;
        }
        
        public AnimationCurve stabilizerFireCurve, stabilizerReturnCurve;
        
        private Coroutine _fireStabilizerCoroutine;
        private Vector3 _fireStartPosition;
        private bool _isFiring;

        public void FireStabilizer()
        {
            if (!_isPointingAtSegment || _isFiring || _isResetting)
            {
                return;
            }

            _isFiring = true;
            rotationMovementIsPaused = true;

            var startPosition = _transform.position;
            _fireStartPosition = startPosition;
            _fireStabilizerCoroutine =  StartCoroutine
            (
                MoveStabilizerCoroutine
                    (
                        startPosition,
                        _centerPosition,
                        stabilizerFireCurve,
                        maxStabilizationTime
                    )
            );
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            StopCoroutine(_fireStabilizerCoroutine);
            
            var startPosition = _transform.position;
            var distanceFromCenter = Vector3.Distance(_centerPosition, startPosition);
            var timeToMoveBack = maxStabilizationTime
                                 * (1 - distanceFromCenter / _stabilizerOrbitRadiusScaled);
            
            if (other.transform.gameObject.TryGetComponent(out CoreSegment segment))
            {
                segment.InterruptAndPutBack(maxStabilizationTime - (1 - timeToMoveBack));
            }

            StartCoroutine
            (
                MoveStabilizerCoroutine
                (
                    startPosition, 
                    _fireStartPosition, 
                    stabilizerReturnCurve, 
                    timeToMoveBack
                )
            );

            StartCoroutine(ResetStatesAfterTime(timeToMoveBack));
        }

        [SerializeField] private Rigidbody2D stabilizerBody;
        
        private IEnumerator MoveStabilizerCoroutine(Vector3 startPosition, 
            Vector3 returnPosition, AnimationCurve stabilizerCurve, float timeToMoveBack)
        {
            stabilizerBody.freezeRotation = true;
            
            IEnumerator currentEnumerator = Utility.LerpVector3
            (
                startPosition,
                returnPosition,
                stabilizerCurve,
                timeToMoveBack
            );
            
            while (currentEnumerator.MoveNext())
            {
                _transform.position = (Vector3) currentEnumerator.Current!;

                yield return null;
            }

            stabilizerBody.freezeRotation = false;
        }

        private IEnumerator ResetStatesAfterTime(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);

            _isFiring = false;
            rotationMovementIsPaused = false;
        }
    }
}