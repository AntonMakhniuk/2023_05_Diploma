using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Player.Movement.Drone_Movement
{
    [Serializable]
    public class EngineGroup : MonoBehaviour
    {
        public List<VisualEffect> engines = new();
    }

    public enum EngineGroupType
    {
        Top, Bottom, Left, Right, Front, Back
    }
}