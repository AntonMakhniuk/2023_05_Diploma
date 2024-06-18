using System.Collections.Generic;
using ResourceNodes;
using Scriptable_Object_Templates;
using Scriptable_Object_Templates.Resources;
using Wagons.Inventory;

namespace Instruments.Miscellaneous
{
    public interface ICollector
    {
        public List<ItemType> CollectableTypes { get; }

        public virtual void Collect(ICollectable itemToCollect)
        {
            InventoryManager.Instance.AddItem(itemToCollect.Resource, itemToCollect.Quantity, null);
            itemToCollect.Dispose();
        }
    }
}