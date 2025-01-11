using NaughtyAttributes;
using UnityEngine;

namespace Environment.Level_Pathfinding.Level_Boundary
{
    [CreateAssetMenu(fileName = "Level Boundary Data", menuName = "Level Data/Level Boundary Data")]
    public class LevelBoundaryData : ScriptableObject
    {
        [Foldout("Zone Radii")]
        [Tooltip("Radius of the free movement zone.")]
        [Range(10f, 1500f)] 
        public float zone1Radius = 50f;

        [Foldout("Zone Radii")]
        [Tooltip("Radius of the deceleration zone.")]
        [Range(10f, 3000f)]
        public float zone2Radius = 100f;

        [Foldout("Zone Radii")]
        [Tooltip("Radius of the forced alignment zone.")]
        [Range(10f, 4500f)]
        public float zone3Radius = 150f;
    }
}