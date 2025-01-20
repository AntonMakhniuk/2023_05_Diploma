using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Miscellaneous.Raycast_Pathfinding
{
    public class TargetFollower : MonoBehaviour
    {
        [Foldout("Pathfinding Data")] [SerializeField]
        private float raycastOffset = 1f;
        [Foldout("Pathfinding Data")] [SerializeField]
        private float raycastLength = 10f;
        [Foldout("Pathfinding Data")] 
        public GameObject target;
        [Foldout("Pathfinding Data")] [SerializeField] [MinMaxSlider(0.1f, 25f)]
        private Vector2 acceptableDistanceFromTarget = new(3f, 5f);

        [Foldout("Movement Data")] [SerializeField]
        private float maxMovementSpeed = 100f;
        [Foldout("Movement Data")] [SerializeField]
        private float accelerationRate = 10f;
        [Foldout("Movement Data")] [SerializeField]
        private float decelerationRate = 10f;
        [Foldout("Movement Data")] [SerializeField]
        private float maxRotationSpeed = 25f;
        [Foldout("Movement Data")] [SerializeField]
        private float rotationAccelerationRate = 10f;
        [Foldout("Movement Data")] [SerializeField]
        private float rotationDecelerationRate = 10f;
        [Foldout("Movement Data")] [SerializeField]
        private float maxDifferenceBeforeDeceleration = 1f;

        [Foldout("Debug Data")] [SerializeField]
        private bool showRays;
        
        private Coroutine _movementCoroutine, _slowDownCoroutine;
        private float _currentMovementSpeed, _currentRotationSpeed;
        private Quaternion _rotationDirection;

        public void MoveTo(GameObject newTarget)
        {
            target = newTarget;
            _movementCoroutine ??= StartCoroutine(MovementCoroutine());
        }

        public void Stop()
        {
            _slowDownCoroutine ??= StartCoroutine(SlowDownCoroutine());
        }

        private IEnumerator MovementCoroutine()
        {
            // Pathfinding first as it modifies rotation data
            FixedUpdatePathfinding();
            FixedUpdateRotation();

            yield return new WaitForFixedUpdate();
        }

        private IEnumerator SlowDownCoroutine()
        {
            while (!Mathf.Approximately(_currentMovementSpeed, 0f))
            {
                // Code for slowing down
                
                yield return new WaitForFixedUpdate();
            }
        }

        private void FixedUpdateRotation()
        {
            var angleDifference = Quaternion.Angle(transform.rotation, _rotationDirection);
            
            if (angleDifference > maxDifferenceBeforeDeceleration)
            {
                _currentRotationSpeed = Mathf.MoveTowards(
                    _currentRotationSpeed,
                    maxRotationSpeed,
                    rotationAccelerationRate * Time.fixedDeltaTime
                );
            }
            else
            {
                _currentRotationSpeed = Mathf.MoveTowards(
                    _currentRotationSpeed,
                    0f,
                    rotationDecelerationRate * Time.fixedDeltaTime
                );
            }
            
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                _rotationDirection,
                _currentRotationSpeed * Time.fixedDeltaTime
            );
        }
        
        private void FixedUpdatePathfinding()
        {
            var position = transform.position;

            var rayLeft = new Ray(position, position - transform.right * raycastOffset);
            var rayRight = new Ray(position, position + transform.right * raycastOffset);
            var rayUp = new Ray(position, position + transform.up * raycastOffset);
            var rayDown = new Ray(position, position - transform.up * raycastOffset);

            if (showRays)
            {
                Debug.DrawRay(position, position - transform.right * raycastOffset, Color.red, raycastLength);
                Debug.DrawRay(position, position + transform.right * raycastOffset, Color.red, raycastLength);
                Debug.DrawRay(position, position + transform.up * raycastOffset, Color.red, raycastLength);
                Debug.DrawRay(position, position - transform.up * raycastOffset, Color.red, raycastLength);
            }

            var directionOffset = Vector3.zero;

            if (Physics.Raycast(rayLeft, out _, raycastLength))
            {
                directionOffset += transform.right;
            }
            else if (Physics.Raycast(rayRight, out _, raycastLength))
            {
                directionOffset -= transform.right;
            }
            
            if (Physics.Raycast(rayUp, out _, raycastLength))
            {
                directionOffset -= transform.up;
            }
            else if (Physics.Raycast(rayDown, out _, raycastLength))
            {
                directionOffset += transform.up;
            }
            
            var direction = target.transform.position - transform.position;
            _rotationDirection = Quaternion.LookRotation(direction);
            
            var eulerRotation = _rotationDirection.eulerAngles;
            eulerRotation += directionOffset * (_currentRotationSpeed * Time.fixedDeltaTime);
            _rotationDirection = Quaternion.Euler(eulerRotation);
        }

        private bool CheckIfTargetInRange()
        {
            var distance = Vector3.Distance(transform.position, target.transform.position);
            
            return acceptableDistanceFromTarget.x <= distance && distance <= acceptableDistanceFromTarget.y;
        }
    }
}