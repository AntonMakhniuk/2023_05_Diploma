using AYellowpaper.SerializedCollections;
using Scriptable_Object_Templates.Resources;
using UnityEngine;

namespace Scriptable_Object_Templates.Singletons.Dictionaries
{
    [CreateAssetMenu(fileName = "ResourcePrefabDictionary", menuName = "GameData/ResourcePrefabDictionary")]
    public class ResourceTypePrefabDictionary : ScriptableSingleton<ResourceTypePrefabDictionary>
    {
        [SerializedDictionary("Resource Type", "Associated Prefab")]
        public SerializedDictionary<ResourceType, GameObject> dictionary;
    }
}