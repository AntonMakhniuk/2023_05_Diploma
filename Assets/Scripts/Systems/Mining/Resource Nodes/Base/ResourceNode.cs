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

        private bool _isBeingDestroyed;
        
        public virtual void InitiateDestroy()
        {
            if (_isBeingDestroyed)
            {
                return;
            }
            
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
            }
           
            Debug.Log(111 + " " + this);
            
            destroyed?.Invoke(this);

            Destroy(gameObject);
            
            _isBeingDestroyed = true;
        }

        protected virtual void OnDestroy()
        {
            
        }
    }

    public enum ResourceNodeType
    {
        GasField, Asteroid
    }
}