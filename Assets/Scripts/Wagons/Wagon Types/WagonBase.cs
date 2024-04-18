using Miscellaneous;
using UnityEngine;
using Wagons.Miscellaneous;

namespace Wagons.Wagon_Types
{
    public abstract class WagonBase : MonoBehaviour, IWagon
    {
        public MassComponent massComponent;
        public float baseMass;
        public WagonType wagonType;
        public JointComponent frontJoint, backJoint;

        private void Start()
        {
            massComponent.SetBaseMass(baseMass);
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
    }

    public interface IWagon
    {
        public WagonBase GetWagon();

        public WagonType GetWagonType();
        
        public void DeleteWagon();
    }
    
    public enum WagonType
    {
        General, Storage
    }
}
