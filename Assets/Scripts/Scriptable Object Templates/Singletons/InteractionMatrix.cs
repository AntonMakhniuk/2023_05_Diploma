using AYellowpaper.SerializedCollections;
using Scriptable_Object_Templates.Resources;
using Systems.Mining.Interactions;
using Tools.Base_Tools;
using UnityEngine;

namespace Scriptable_Object_Templates.Singletons
{
    [CreateAssetMenu(fileName = "Interaction Matrix", menuName = "GameData/InteractionMatrix")]
    public class InteractionMatrix : ScriptableSingleton<InteractionMatrix>
    {
        [SerializedDictionary("Matrix Key", "Associated Interaction")]
        public SerializedDictionary<(ToolType, ResourceType, ResourceState), InteractionEffect> matrix;    
    }
}