using System;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Scriptable_Object_Templates.Resources;
using UnityEngine;

namespace Building.Buildings.Base_Classes
{
    public class BaseBlueprint : MonoBehaviour
    {
        [SerializedDictionary("Item", "Quantity")]
        public readonly SerializedDictionary<ItemBase, float> RequiredItemQuantityDict = new();
        
        public event EventHandler<BaseBlueprint> OnBlueprintResourcesFulfilled; 
        
        // Returns quantity added to the blueprint
        public float AddResources(ItemBase item, float quantity)
        {
            if (!RequiredItemQuantityDict.Keys.Contains(item))
            {
                return 0;
            }

            var added = Math.Min(RequiredItemQuantityDict[item], quantity);

            RequiredItemQuantityDict[item] -= added;

            if (RequiredItemQuantityDict.Values.All(v => v == 0))
            {
                OnBlueprintResourcesFulfilled?.Invoke(this, this);
            }
            
            return added;
        }
    }
}