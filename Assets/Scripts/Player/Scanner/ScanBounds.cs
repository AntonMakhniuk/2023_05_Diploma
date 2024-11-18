using UnityEngine;

namespace Player.Scanner
{
    public class ScanBounds : MonoBehaviour
    {
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