using System;
using System.Collections.Generic;
using System.Linq;
using Miscellaneous;
using Scriptable_Object_Templates;
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
        
        public Dictionary<ItemBase, float> ItemDictionary = new();

        // Derived attributes
        [HideInInspector] public float occupiedCapacity;
        [HideInInspector] public float freeCapacity;

        private void UpdateCapacities()
        {
            float occupiedTemp = ItemDictionary.Sum(item => item.Key.volume * item.Value);

            occupiedCapacity = occupiedTemp;
            freeCapacity = maxCapacity - occupiedCapacity;
        }

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

            float amountAdded = Mathf.Min(itemCount, freeCapacity);
            
            if (ItemDictionary.ContainsKey(item))
            {
                ItemDictionary[item] += amountAdded;
            }
            else
            {
                ItemDictionary.Add(item, amountAdded);
            }
            
            UpdateCapacities();

            return amountAdded;
        }

        // Takes out the provided count, or maximum available amount if there isn't enough
        // returns the amount taken out
        public float TakeOutItem(ItemBase item, float itemCount)
        {
            float takenOutCount;
            
            if (!ItemDictionary.ContainsKey(item))
            {
                return 0;
            }

            if (ItemDictionary[item] < itemCount)
            {
                takenOutCount = ItemDictionary[item];
                ItemDictionary[item] = 0;
            }
            else
            {
                takenOutCount = itemCount;
                ItemDictionary[item] -= itemCount;
            }

            UpdateCapacities();
            
            return takenOutCount;
        }
    }
}