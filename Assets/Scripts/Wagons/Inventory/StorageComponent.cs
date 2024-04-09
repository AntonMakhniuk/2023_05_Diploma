using System;
using System.Collections.Generic;
using System.Linq;
using Scriptable_Object_Templates;
using UnityEngine;

namespace Wagons.Inventory
{
    [Serializable]
    public class StorageComponent : MonoBehaviour
    {
        public ItemType[] allowedItemTypes;
        public float maxCapacity;
        
        public Dictionary<ItemBase, float> itemDictionary = new();

        // Derived attributes
        [HideInInspector] public float occupiedCapacity;
        [HideInInspector] public float freeCapacity;

        private void UpdateCapacities()
        {
            float occupiedTemp = itemDictionary.Sum(item => item.Key.volume * item.Value);

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
            
            if (itemDictionary.ContainsKey(item))
            {
                itemDictionary[item] += amountAdded;
            }
            else
            {
                itemDictionary.Add(item, amountAdded);
            }
            
            UpdateCapacities();

            return amountAdded;
        }

        // Takes out the provided count, or maximum available amount if there isn't enough
        // returns the amount taken out
        public float TakeOutItem(ItemBase item, float itemCount)
        {
            float takenOutCount;
            
            if (!itemDictionary.ContainsKey(item))
            {
                return 0;
            }

            if (itemDictionary[item] < itemCount)
            {
                takenOutCount = itemDictionary[item];
                itemDictionary[item] = 0;
            }
            else
            {
                takenOutCount = itemCount;
                itemDictionary[item] -= itemCount;
            }

            UpdateCapacities();
            
            return takenOutCount;
        }
    }
}