using UnityEngine;

namespace Scriptable_Object_Templates.Building
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "Building/BuildingData")]
    public class BuildingData : ScriptableObject
    {
        public Sprite icon;
        public string label;
        public BuildingType type;
        public GameObject prefab;
    }
    
    public enum BuildingType
    {
        Teleporter, RefillStation, Accelerator
    }
}
