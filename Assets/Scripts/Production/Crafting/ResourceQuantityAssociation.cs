using System;
using Scriptable_Object_Templates;

namespace Production.Crafting
{
    [Serializable]
    public class ResourceQuantityAssociation
    {
        public Resource resource;
        public int quantity;
    }
}