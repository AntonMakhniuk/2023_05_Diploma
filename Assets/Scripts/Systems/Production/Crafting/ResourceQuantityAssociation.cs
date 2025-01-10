using System;
using Scriptable_Object_Templates.Resources;

namespace Production.Crafting
{
    [Serializable]
    public class ResourceQuantityAssociation
    {
        public ResourceData resourceData;
        public int quantity;
    }
}