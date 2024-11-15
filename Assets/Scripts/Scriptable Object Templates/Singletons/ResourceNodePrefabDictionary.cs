using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Systems.Mining.Resource_Nodes.Base;
using UnityEngine;

namespace Scriptable_Object_Templates.Singletons
{
    [CreateAssetMenu(fileName = "Resource Node Prefab Dictionary", menuName = "GameData/ResourceNodePrefabDictionary")]
    public class ResourceNodePrefabDictionary : ScriptableSingleton<ResourceNodePrefabDictionary>
    {
        [SerializedDictionary("Resource Node Type", "Associated Prefabs")]
        public SerializedDictionary<ResourceNodeType, List<GameObject>> dictionary;
        
        public GameObject GetRandomPrefabByType(ResourceNodeType type)
        {
            return dictionary[type][Random.Range(0, dictionary[type].Count)];
        }
    }
}