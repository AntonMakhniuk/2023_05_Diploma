using UnityEngine;

namespace Player.Ship.Tools.Scanner
{
    public class ScanBounds : MonoBehaviour
    {
        [SerializeField] private bool lockRotation;
        
        private Quaternion _initialRotation;

        private void Start()
        {
            _initialRotation = transform.parent.rotation;
        }

        private void OnEnable()
        {
            _initialRotation = transform.parent.rotation;
        }

        private void LateUpdate()
        {
            if (lockRotation)
            {
                transform.rotation = _initialRotation;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Scannable>(out var scannable))
            {
                scannable.Toggle();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Scannable>(out var scannable))
            {
                scannable.Toggle();
            }
        }
    }
}