using System;
using UnityEngine;

// DEPRECATED

namespace Player.Movement.Miscellaneous
{
    public class FuelSystem : MonoBehaviour
    {
        private const float LowFuelThreshold = 20f;
        
        [SerializeField] private float maxFuelCapacity = 100f;
        public float MaxFuelCapacity => maxFuelCapacity;
        
        private float _currentFuelLevel;
        public float CurrentFuelLevel => _currentFuelLevel;

        private event EventHandler<float> OnFuelLevelChanged;
        
        private void Awake()
        {
            _currentFuelLevel = maxFuelCapacity;
            
            OnFuelLevelChanged?.Invoke(this, _currentFuelLevel);
        }

        public void AddFuel(float amount)
        {
            _currentFuelLevel = Mathf.Min(_currentFuelLevel + amount, maxFuelCapacity);
            
            OnFuelLevelChanged?.Invoke(this, _currentFuelLevel);
        }

        public void ConsumeFuel(float amount)
        {
            _currentFuelLevel = Mathf.Max(_currentFuelLevel - amount, 0);
            
            OnFuelLevelChanged?.Invoke(this, _currentFuelLevel);
        }
    }
}
