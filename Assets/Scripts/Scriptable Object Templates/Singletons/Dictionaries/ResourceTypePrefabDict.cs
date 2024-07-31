using AYellowpaper.SerializedCollections;
using Scriptable_Object_Templates.Resources;
using UnityEngine;

namespace Scriptable_Object_Templates.Singletons.Dictionaries
{
    [CreateAssetMenu(fileName = "ResourcePrefabDictionary", menuName = "GameData/ResourcePrefabDictionary")]
    public class ResourceTypePrefabDict : ScriptableSingleton<ResourceTypePrefabDict>
    {
        [SerializedDictionary("Resource Type", "Associated Prefab")]
        public SerializedDictionary<ResourceType, GameObject> resourceTypePrefabDictionary;
    }
}