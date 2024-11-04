using UnityEngine;

namespace Scriptable_Object_Templates.Resources
{
    [CreateAssetMenu(fileName = "Resource", menuName = "Crafting/Resource")]
    public class Resource : ItemBase
    {
        public ResourceType resourceType;
    }

    public enum ResourceType
    {
        FuelGas, SpaceOre
    }

    public enum ResourceState
    {
        Gas, Solid, Liquid
    }
}