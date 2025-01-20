using AYellowpaper.SerializedCollections;
using Environment.Level_Pathfinding.Level_Boundary;
using UnityEngine;

namespace Scriptable_Object_Templates.Singletons
{
    [CreateAssetMenu(fileName = "Level Boundary Dictionary", menuName = "Level Data/Level Boundary Dictionary")]
    public class LevelBoundaryDictionary : ScriptableSingleton<LevelBoundaryDictionary>
    {
        [SerializedDictionary("Scene Name", "Associated Boundary Data")]
        public SerializedDictionary<string, LevelBoundaryData> dictionary;
    }
}