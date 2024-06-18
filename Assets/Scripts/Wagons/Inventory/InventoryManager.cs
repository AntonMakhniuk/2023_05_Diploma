using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Scriptable_Object_Templates;
using Scriptable_Object_Templates.Resources;
using UnityEngine;
using Wagons.Systems;
using Wagons.Wagon_Types;

namespace Wagons.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;
        
        public WagonManager playerWagonManager;
        
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
        
        private void Start()
        {
            // TODO: add some kind of link to storage that can be on the ship as well
            
            foreach (var wagon in playerWagonManager.GetAllAttachedWagonsOfType<IStorageWagon>())
            {
                _shipStorageComponents.AddRange(wagon.GetStorageComponents());
            }

            playerWagonManager.OnWagonListChanged += UpdateStorageComponentList;
        }

        private void UpdateStorageComponentList(IWagon[] wagons, WagonOperationType operationType)
        {
            var storageWagons = new List<IStorageWagon>();
            
            foreach (var wagon in wagons)
            {
                if (wagon is IStorageWagon storageWagon)
                {
                    storageWagons.Add(storageWagon);
                }
            }

            var storageComponents = 
                storageWagons
                .Select(wagon => wagon.GetStorageComponents())
                .ToList()
                .SelectMany(comp => comp);
            
            switch (operationType)
            {
                case WagonOperationType.Creation:
                case WagonOperationType.Addition:
                {
                    _shipStorageComponents.AddRange(storageComponents);

                    break;
                }
                case WagonOperationType.Removal:
                {
                    foreach (var storageComponent in storageComponents)
                    {
                        _shipStorageComponents.Remove(storageComponent);
                    }

                    break;
                }
            }
        }

        public float AddItem(ItemBase item, float itemCount, [CanBeNull] StorageComponent specificComponent)
        {
            if (specificComponent != null)
            {
                return specificComponent.AddItem(item, itemCount);
            }
            
            float itemTotal = 0;

            foreach (var storageComponent in _shipStorageComponents
                         .Where(sc => sc.allowedItemTypes.Contains(item.type)).ToList())
            {
                itemTotal += storageComponent.AddItem(item, itemCount - itemTotal);

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