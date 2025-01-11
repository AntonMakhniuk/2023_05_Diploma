namespace Player.Inventory.Drone_Based_Storage
{
    public class IdleState : BaseStorageDroneState
    {
        public IdleState(StorageDrone context) : base(context)
        {
        }

        public override void Enter()
        {
            var spawnPoint = Context.parentStorage.GetSpawnPointByDrone(Context);

            if (Context.transform.position != spawnPoint.position)
            {
                //Context.MoveTo(spawnPoint);
            }
        }

        public override void Update()
        {
            //
        }

        public override void Exit()
        {
            //
        }
    }
}