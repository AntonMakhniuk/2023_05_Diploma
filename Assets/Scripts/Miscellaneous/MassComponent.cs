using System;
using UnityEngine;

namespace Miscellaneous
{
    public class MassComponent : MonoBehaviour
    {
        [SerializeField] private float baseMass;
        [SerializeField] private new Rigidbody rigidbody;
        
        [Header("Shown here for easier access, treat as read-only")]
        public float currentMass;

        private void Awake()
        {
            SetBaseMass(baseMass);
        }

        public void SetBaseMass(float newBaseMass)
        {
            baseMass = newBaseMass;
            
            ChangeMassByModifier(newBaseMass);
        }

        public void ChangeMassByModifier(float modifier)
        {
            if (currentMass + modifier < 0)
            {
                throw new Exception($"Trying to make weight of {nameof(gameObject)} negative");
            }

            currentMass += modifier;
            rigidbody.mass = currentMass;
        }
        
        public void SetMass(float newValue)
        {
            if (newValue < 0)
            {
                throw new Exception($"Trying to set weight of {nameof(gameObject)} to negative value");
            }

            currentMass = newValue;
            rigidbody.mass = currentMass;
        }
    }
}