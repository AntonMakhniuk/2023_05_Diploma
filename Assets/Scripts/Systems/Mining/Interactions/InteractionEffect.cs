using Tools.Base_Tools;
using UnityEngine;

namespace Systems.Mining.Interactions
{
    public abstract class InteractionEffect : ScriptableObject
    {
        public abstract void Apply(ToolType tool, Material material);
    }
}