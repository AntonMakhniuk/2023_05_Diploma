using UnityEngine;

namespace Wagons.Miscellaneous
{
    public class JointComponent : MonoBehaviour
    {
        [SerializeField] private ConfigurableJoint joint;

        public void Connect(JointComponent jointToConnect)
        {
            if (joint.connectedBody != null)
            {
                Debug.Log("Tried to connect to a joint that already has a connected object");
                
                return;
            }

            joint.connectedBody = jointToConnect.GetComponent<Rigidbody>();
        }

        public void Disconnect()
        {
            joint.connectedBody = null;
        }
    }
}