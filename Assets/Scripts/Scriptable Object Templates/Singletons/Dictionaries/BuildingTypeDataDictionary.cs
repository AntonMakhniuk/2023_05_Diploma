using AYellowpaper.SerializedCollections;
using Building.Structures;
using Scriptable_Object_Templates.Building;
using UnityEngine;

namespace Scriptable_Object_Templates.Singletons.Dictionaries
{
    [CreateAssetMenu(fileName = "BuildingTypeDataDictionary", menuName = "GameData/BuildingTypeDataDictionary")]
    public class BuildingTypeDataDictionary : ScriptableSingleton<BuildingTypeDataDictionary>
    {
        [SerializedDictionary("Building Type", "Associated Data")]
        public SerializedDictionary<BuildingType, BuildingData> dictionary;
    }
}