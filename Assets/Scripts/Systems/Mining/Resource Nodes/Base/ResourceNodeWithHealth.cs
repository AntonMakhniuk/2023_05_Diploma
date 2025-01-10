using System.Collections.Generic;
using NaughtyAttributes;
using Player.Ship.Tools.Base_Tools;
using UnityEngine;

namespace Systems.Mining.Resource_Nodes.Base
{
    public abstract class ResourceNodeWithHealth : ResourceNode
    {
        [Header("Health Data")]
        [SerializeField] private float maxHealth;
        [ReadOnly] [SerializeField] private float currentHealth;
        [SerializeField] private List<ToolType> damagingToolList = new();

        protected virtual void Start()
        {
            currentHealth = maxHealth;
        }

        public void Interact(ToolType toolType, float damage)
        {
            TakeDamage(toolType, damage);
            Interact(toolType);
        }

        private void TakeDamage(ToolType toolType, float damage)
        {
            if (!damagingToolList.Contains(toolType))
            {
                return;
            }
            
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                InitiateDestroy();
            }
        }
    }
}