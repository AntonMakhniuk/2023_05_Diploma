using Scriptable_Object_Templates;
using Scriptable_Object_Templates.Resources;
using UnityEngine;

namespace ResourceNodes
{
    public interface ICollectable
    {
        GameObject GameObject { get ; } 
        public Resource Resource { get; }
        public float Quantity { get; set; }

        public abstract void Dispose();
    }
}