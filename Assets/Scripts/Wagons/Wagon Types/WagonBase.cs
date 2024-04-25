using UnityEngine;
using Wagons.Miscellaneous;

namespace Wagons.Wagon_Types
{
    public abstract class WagonBase : MonoBehaviour, IWagon
    {
        [HideInInspector] public WagonType wagonType;
        public JointComponent backJoint;

        private Rigidbody _rigidbody;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public virtual WagonBase GetWagon()
        {
            return this;
        }

        public WagonType GetWagonType()
        {
            return wagonType;
        }

        public virtual void DeleteWagon()
        {
            Destroy(this);
        }

        public void SetDragValues(float drag, float angularDrag)
        {
            _rigidbody.drag = drag;
            _rigidbody.angularDrag = angularDrag;
        }
    }

    public interface IWagon
    {
        public WagonBase GetWagon();

        public WagonType GetWagonType();
        
        public void DeleteWagon();

        public void SetDragValues(float drag, float angularDrag);
    }
    
    public enum WagonType
    {
        PlayerShip, General, Storage
    }
}
