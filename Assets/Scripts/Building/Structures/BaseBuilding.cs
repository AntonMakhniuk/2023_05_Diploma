using UnityEngine;

namespace Building.Structures
{
    public abstract class BaseBuilding : MonoBehaviour
    {
        public GameObject blueprint;
        public GameObject finished;
    }

    public enum BuildingType
    {
        Teleporter, Accelerator, RefillStation
    }
}