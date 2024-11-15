using System;
using Systems.Mining.Transitions.Transition_Addons;
using Tools.Base_Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Mining.Resource_Nodes.Base
{
    public abstract class ResourceNode : MonoBehaviour
    {
        [HideInInspector] public ToolType lastInteractedTool;

        [Header("Base case of destruction")]
        public UnityEvent<ResourceNode> destroyed;

        public void Interact(ToolType toolType)
        {
            lastInteractedTool = toolType;
            
            switch (toolType)
            {
                case ToolType.Laser:
                {
                    OnLaserInteraction();

                    break;
                }
                case ToolType.Bomb:
                {
                    OnBombInteraction();

                    break;
                }
            }
        }
        
        protected abstract void OnLaserInteraction();

        protected abstract void OnBombInteraction();

        [Header("Tool-specific cases of destruction")]
        public UnityEvent<ResourceNode> destroyedByLaser;
        public UnityEvent<ResourceNode> destroyedByBomb;
        
        private void InitiateDestroy()
        {
            switch (lastInteractedTool)
            {
                case ToolType.Laser:
                {
                    destroyedByLaser?.Invoke(this);

                    break;
                }
                case ToolType.Bomb:
                {
                    destroyedByBomb?.Invoke(this);

                    break;
                }
                default:
                {
                    destroyed?.Invoke(this);
                    
                    break;
                }
            }
        }

        protected virtual void OnDestroy()
        {
            InitiateDestroy();
        }
    }

    public enum ResourceNodeType
    {
        GasField, Asteroid
    }
}