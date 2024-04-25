using Scriptable_Object_Templates;
using Wagons.Inventory;

namespace Wagons.Wagon_Types
{
    public class WagonMonoStorage : WagonBase, IStorageWagon
    {
        public StorageComponent storageComponent;
        
        protected override void Awake()
        {
            base.Awake();
            
            wagonType = WagonType.Storage;
        }
        
        public StorageComponent[] GetStorageComponents()
        {
            return new[] { storageComponent };
        }

        public void AddItem(ItemBase item, float itemCount)
        {
            storageComponent.AddItem(item, itemCount);
        }

        public float TakeOutItem(ItemBase item, float itemCount)
        {
            return storageComponent.TakeOutItem(item, itemCount);
        }
    }

    public interface IStorageWagon : IWagon
    {
        public StorageComponent[] GetStorageComponents();

        public void AddItem(ItemBase item, float itemCount);

        public float TakeOutItem(ItemBase item, float itemCount);
    }
}