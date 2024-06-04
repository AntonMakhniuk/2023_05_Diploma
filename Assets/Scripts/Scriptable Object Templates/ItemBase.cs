using UnityEngine;

namespace Scriptable_Object_Templates
{
    public class ItemBase : ScriptableObject
    {
        public Sprite icon;
        public string label;
        public ItemType type;
        public float mass;
        public float volume;
    }

    public enum ItemType
    {
        Ore, Gas, Material
    }
}