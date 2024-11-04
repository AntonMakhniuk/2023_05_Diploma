using Scriptable_Object_Templates.Resources;
using UnityEngine;

namespace Resource_Nodes
{
    public abstract class ResourceNode : MonoBehaviour
    {
        public Resource associatedResource;
    }

    public enum ResourceNodeType
    {
        GasField, Asteroid
    }
}