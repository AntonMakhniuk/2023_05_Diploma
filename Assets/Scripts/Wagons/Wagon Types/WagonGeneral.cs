namespace Wagons.Wagon_Types
{
    public class WagonGeneral : WagonBase
    {
        protected override void Awake()
        {
            base.Awake();
            
            wagonType = WagonType.General;
        }
    }
}