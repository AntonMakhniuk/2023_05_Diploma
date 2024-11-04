namespace Wagons.Wagon_Types
{
    public class WagonPlayerShip : WagonBase
    {
        protected override void Awake()
        {
            base.Awake();
            
            wagonType = WagonType.PlayerShip;
        }
    }
}