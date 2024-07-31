using UnityEngine;

namespace Miscellaneous
{
    public class RotationMatcher : MonoBehaviour
    {
        private Transform _target;

        private void Awake()
        {
            if (Camera.main != null)
            {
                _target = Camera.main.transform;
            }
        }

        private void LateUpdate()
        {
            transform.rotation = _target.rotation;
        }
    }
}
