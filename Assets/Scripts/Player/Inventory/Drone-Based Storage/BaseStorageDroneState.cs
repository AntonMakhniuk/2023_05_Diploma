namespace Player.Inventory.Drone_Based_Storage
{
    public abstract class BaseStorageDroneState
    {
        protected readonly StorageDrone Context;

        protected BaseStorageDroneState(StorageDrone context)
        {
            Context = context;
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }

    public enum StorageDroneState
    {
        Idle, Approaching, Attracting, Delivering
    }
}