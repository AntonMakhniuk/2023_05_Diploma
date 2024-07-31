using UnityEngine;

namespace Scriptable_Object_Templates.Resources
{
    public class ItemBase : ScriptableObject
    {
        public Sprite icon;
        public string label;
        public ItemCategory category;
        public float mass;
        public float volume;
    }

    public enum ItemCategory
    {
        Ore, Gas, Material
    }
}