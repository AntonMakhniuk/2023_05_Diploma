using System;
using System.Collections.Generic;
using System.Linq;
using Scriptable_Object_Templates.Systems.Mining.Resource_Data;
using UnityEngine;

namespace Player.Inventory
{
    [Serializable]
    public class StorageComponent : MonoBehaviour
    {
        public ItemCategory[] allowedItemTypes;
        public float maxCapacity;

        private List<ItemStack> _items = new();
        public List<ItemStack> Items => new(_items);

        // Derived attributes
        public float OccupiedCapacity => _items.Sum(item => item.DVolume);
        public float FreeCapacity => maxCapacity - OccupiedCapacity;

        private void Start()
        {
            InventoryManager.Instance.RegisterComponent(this);
        }

        // Adds the provided count, or maximum available amount if there isn't enough space
        // returns the amount added
        public float AddItem(ItemBase item, float itemCount)
        {
            if (!allowedItemTypes.Contains(item.category))
            {
                Debug.Log($"Tried to add invalid item type: {item.category} " +
                          $"to storage component {this} that only allows {allowedItemTypes}");
                
                return 0;
            }

            if (FreeCapacity <= 0)
            {
                //TODO: Add player notification that max capacity has been reached
                
                return 0;
            }
            
            var amountAdded = Mathf.Min(itemCount * item.volume, FreeCapacity);
            
            if (_items.Select(st => st.item).Contains(item))
            {
                _items.Single(st => st.item == item).quantity += amountAdded;
            }
            else
            {
                _items.Add(new ItemStack(item, amountAdded));
            }

            return amountAdded;
        }
        
        // Same as AddItem, except it doesn't modify the actual storage
        public float PeekAddItem(ItemBase item, float itemCount)
        {
            if (!allowedItemTypes.Contains(item.category))
            {
                Debug.Log($"Tried to add invalid item type: {item.category} " +
                          $"to storage component {this} that only allows {allowedItemTypes}");
                
                return 0;
            }

            if (FreeCapacity <= 0)
            {
                //TODO: Add player notification that max capacity has been reached
                
                return 0;
            }
            
            // Theoretical amount added
            return Mathf.Min(itemCount * item.volume, FreeCapacity);
        }

        // Takes out the provided count, or maximum available amount if there isn't enough
        // returns the amount taken out
        public float TakeOutItem(ItemBase item, float itemCount)
        {
            var requestedItem = _items.SingleOrDefault(st => st.item == item);
            
            if (requestedItem == null)
            {
                Debug.Log("Tried to take out item that doesn't exist in player inventory.");
                
                return 0;
            }
            
            float takenOutCount;
            
            if (requestedItem.quantity < itemCount)
            {
                takenOutCount = requestedItem.quantity;
                requestedItem.quantity = 0;
            }
            else
            {
                takenOutCount = itemCount;
                requestedItem.quantity -= itemCount;
            }
            
            return takenOutCount;
        }
        
        // Same as TakeOutItem, except it doesn't modify the actual storage
        public float PeekTakeOutItem(ItemBase item, float itemCount)
        {
            var requestedItem = _items.SingleOrDefault(st => st.item == item);
            
            if (requestedItem == null)
            {
                Debug.Log("Tried to take out item that doesn't exist in player inventory.");
                
                return 0;
            }

            var takenOutCount = requestedItem.quantity < itemCount ? requestedItem.quantity : itemCount;
            
            return takenOutCount;
        }

        private void OnDestroy()
        {
            InventoryManager.Instance.UnregisterComponent(this);
        }
    }
}