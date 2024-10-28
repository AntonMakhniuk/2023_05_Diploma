using AYellowpaper.SerializedCollections;
using Building.Buildings.Base_Classes;
using Scriptable_Object_Templates.Building;
using UnityEngine;

namespace Scriptable_Object_Templates.Singletons
{
    [CreateAssetMenu(fileName = "Building Prefab Dictionary", menuName = "GameData/BuildingPrefabDictionary")]
    public class BuildingPrefabDictionary : ScriptableSingleton<BuildingPrefabDictionary>
    {
        [SerializedDictionary("Building Type", "Associated Data")]
        public SerializedDictionary<BuildingType, BuildingData> dictionary;
    }
}