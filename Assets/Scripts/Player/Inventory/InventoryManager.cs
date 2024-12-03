using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Scriptable_Object_Templates.Systems.Mining.Resource_Data;
using UnityEngine;

namespace Player.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;
        
        private readonly List<StorageComponent> _shipStorageComponents = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void RegisterComponent(StorageComponent newComponent)
        {
            if (!_shipStorageComponents.Contains(newComponent))
            {
                _shipStorageComponents.Add(newComponent);
            }
        }

        public void UnregisterComponent(StorageComponent component)
        {
            if (!_shipStorageComponents.Contains(component))
            {
                return;
            }

            _shipStorageComponents.Remove(component);
        }
        
        public float AddItem(ItemBase item, float itemCount, [CanBeNull] StorageComponent specificComponent)
        {
            if (specificComponent != null)
            {
                return specificComponent.AddItem(item, itemCount);
            }
            
            float itemTotal = 0;

            foreach (var storageComponent in _shipStorageComponents
                         .Where(sc => sc.allowedItemTypes.Contains(item.category)).ToList())
            {
                itemTotal += storageComponent.AddItem(item, itemCount - itemTotal);

                if (Math.Abs(itemTotal - itemCount) < 0.001)
                {
                    break;
                }
            }
            
            return itemTotal;
        }
        
        public float PeekAddItem(ItemBase item, float itemCount, [CanBeNull] StorageComponent specificComponent)
        {
            if (specificComponent != null)
            {
                return specificComponent.PeekAddItem(item, itemCount);
            }
            
            float itemTotal = 0;

            foreach (var storageComponent in _shipStorageComponents
                         .Where(sc => sc.allowedItemTypes.Contains(item.category)).ToList())
            {
                itemTotal += storageComponent.PeekAddItem(item, itemCount - itemTotal);

                if (Math.Abs(itemTotal - itemCount) < 0.001)
                {
                    break;
                }
            }
            
            return itemTotal;
        }
        
        public float TakeOutItem(ItemBase item, float itemCount, [CanBeNull] StorageComponent specificComponent)
        {
            if (specificComponent != null)
            {
                return specificComponent.TakeOutItem(item, itemCount);
            }
            
            float itemTotal = 0;

            foreach (var storageComponent in _shipStorageComponents)
            {
                itemTotal += storageComponent.TakeOutItem(item, itemCount - itemTotal);

                if (Math.Abs(itemTotal - itemCount) < 0.001)
                {
                    break;
                }
            }

            return itemTotal;
        }

        public float PeekTakeOutItem(ItemBase item, float itemCount, [CanBeNull] StorageComponent specificComponent)
        {
            if (specificComponent != null)
            {
                return specificComponent.PeekTakeOutItem(item, itemCount);
            }
            
            float itemTotal = 0;

            foreach (var storageComponent in _shipStorageComponents)
            {
                itemTotal += storageComponent.PeekTakeOutItem(item, itemCount - itemTotal);

                if (Math.Abs(itemTotal - itemCount) < 0.001)
                {
                    break;
                }
            }

            return itemTotal;
        }
        
        public List<ItemStack> GetAllItems()
        {
            List<ItemStack> combinedItems = new();
            
            foreach (var items in _shipStorageComponents.Select(sc => sc.Items))
            {
                foreach (var stack in items)
                {
                    if (!combinedItems.Contains(stack))
                    {
                        combinedItems.Add(new ItemStack(stack));
                    }
                    else
                    {
                        combinedItems.Single(st => st == stack).quantity += stack.quantity;
                    }
                }
            }
            
            return combinedItems;
        }
    }
}