using System;
using Scriptable_Object_Templates;
using Scriptable_Object_Templates.Resources;

namespace Production.Crafting
{
    [Serializable]
    public class ResourceQuantityAssociation
    {
        public Resource resource;
        public int quantity;
    }
}