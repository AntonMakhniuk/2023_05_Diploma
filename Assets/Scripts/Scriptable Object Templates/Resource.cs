using UnityEngine;

namespace Scriptable_Object_Templates
{
    [CreateAssetMenu(fileName = "Resource", menuName = "Crafting/Resource")]
    public class Resource : ScriptableObject
    {
        public string label;
        public ResourceType type;
        
    }

    public enum ResourceType
    {
        Ore, Gas
    }
}