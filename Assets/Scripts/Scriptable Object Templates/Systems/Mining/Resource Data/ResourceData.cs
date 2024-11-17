using Scriptable_Object_Templates.Systems.Mining.Resource_Data;
using UnityEngine;

namespace Scriptable_Object_Templates.Resources
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "Crafting/ResourceData")]
    public class ResourceData : ItemBase
    {
        public ResourceType resourceType;
    }

    public enum ResourceType
    {
        FuelGas, SpaceOre
    }
}