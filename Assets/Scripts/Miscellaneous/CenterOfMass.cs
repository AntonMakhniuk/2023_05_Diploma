using UnityEngine;

namespace Miscellaneous
{
    [RequireComponent(typeof(Rigidbody))]
    public class CenterOfMass : MonoBehaviour
    {
        [SerializeField] private Vector3 centerOfMass;
        private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            centerOfMass = _rb.centerOfMass;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + transform.rotation * centerOfMass, 1f);
        }
    }
}
