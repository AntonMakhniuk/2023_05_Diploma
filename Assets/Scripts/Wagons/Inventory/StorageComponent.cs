using System;
using System.Collections.Generic;
using System.Linq;
using Miscellaneous;
using Scriptable_Object_Templates;
using Scriptable_Object_Templates.Resources;
using UnityEngine;

namespace Wagons.Inventory
{
    [Serializable]
    public class StorageComponent : MonoBehaviour
    {
        // Add mass calculations
        [HideInInspector] public MassComponent massComponent;
        
        public ItemType[] allowedItemTypes;
        public float maxCapacity;

        private List<ItemStack> _items = new();
        public List<ItemStack> Items => new(_items);

        // Derived attributes
        public float OccupiedCapacity => _items.Sum(item => item.DVolume);
        public float FreeCapacity => maxCapacity - OccupiedCapacity;

        // Adds the provided count, or maximum available amount if there isn't enough space
        // returns the amount added
        public float AddItem(ItemBase item, float itemCount)
        {
            if (!allowedItemTypes.Contains(item.type))
            {
                Debug.Log($"Tried to add invalid item type: {item.type} " +
                          $"to storage component {this} that only allows {allowedItemTypes}");
                
                return 0;
            }

            if (FreeCapacity <= 0)
            {
                //TODO: Add player notification that max capacity has been reached
                
                return 0;
            }
            
            float amountAdded = Mathf.Min(itemCount * item.volume, FreeCapacity);
            
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
    }
}