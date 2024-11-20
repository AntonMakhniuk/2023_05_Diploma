using UnityEngine;

namespace Player.Scanner
{
    public class ScanBounds : MonoBehaviour
    {
        private Quaternion _initialRotation;

        private void Start()
        {
            _initialRotation = transform.rotation;
        }

        private void LateUpdate()
        {
            transform.rotation = _initialRotation;
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