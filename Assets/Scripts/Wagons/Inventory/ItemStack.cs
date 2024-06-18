using System.Collections.Generic;
using Scriptable_Object_Templates;
using Scriptable_Object_Templates.Resources;

namespace Wagons.Inventory
{
    public class ItemStack
    {
        public ItemBase item;
        public float quantity;
        public float DVolume => item.volume * quantity;
        public float DMass => item.mass * quantity;

        public ItemStack(ItemBase item, float quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }

        public ItemStack(ItemStack original)
        {
            item = original.item;
            quantity = original.quantity;
        }

        private sealed class ItemEqualityComparer : IEqualityComparer<ItemStack>
        {
            public bool Equals(ItemStack x, ItemStack y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return Equals(x.item, y.item);
            }

            public int GetHashCode(ItemStack obj)
            {
                return (obj.item != null ? obj.item.GetHashCode() : 0);
            }
        }

        public static IEqualityComparer<ItemStack> ItemComparer { get; } = new ItemEqualityComparer();
    }
}