using NaughtyAttributes;
using UnityEngine;

namespace Player.Ship.Tools.Marker
{
    public class TestMarkerBounds : MonoBehaviour
    {
        private int _collectablesInside;

        public bool IsValid => _collectablesInside > 0;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Collectable>(out var collectable) || collectable.isMarked)
            {
                return;
            }
            
            _collectablesInside++;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Collectable>(out var collectable) || !collectable.isMarked)
            {
                return;
            }
            
            _collectablesInside--;
        }
    }
}