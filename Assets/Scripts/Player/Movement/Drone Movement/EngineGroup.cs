using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

namespace Player.Movement.Drone_Movement
{
    [Serializable]
    public class EngineGroup : MonoBehaviour
    {
        public List<VisualEffect> engineStartEffects = new();
        public List<VisualEffect> engineActiveEffects = new();
        public List<VisualEffect> engineEndEffects = new();
        
        private Dictionary<VisualEffect, bool> activeStates = new();

        private void Awake()
        {
            foreach (var effect in engineStartEffects.Concat(engineActiveEffects).Concat(engineEndEffects))
            {
                activeStates[effect] = false;
            }
        }

        public bool IsEffectActive(VisualEffect effect)
        {
            return activeStates.ContainsKey(effect) && activeStates[effect];
        }

        public void SetEffectActive(VisualEffect effect, bool isActive)
        {
            if (activeStates.ContainsKey(effect))
            {
                activeStates[effect] = isActive;
            }
        }
    }

    public enum EngineGroupType
    {
        Top, Bottom, Left, Right, Front, Back
    }
}