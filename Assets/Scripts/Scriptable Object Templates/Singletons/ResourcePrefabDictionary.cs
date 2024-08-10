using AYellowpaper.SerializedCollections;
using Scriptable_Object_Templates.Resources;
using UnityEngine;

namespace Scriptable_Object_Templates.Singletons
{
    [CreateAssetMenu(fileName = "Resource Prefab Dictionary", menuName = "GameData/ResourcePrefabDictionary")]
    public class ResourcePrefabDictionary : ScriptableSingleton<ResourcePrefabDictionary>
    {
        [SerializedDictionary("Resource Type", "Associated Prefab")]
        public SerializedDictionary<ResourceType, GameObject> dictionary;
    }
}