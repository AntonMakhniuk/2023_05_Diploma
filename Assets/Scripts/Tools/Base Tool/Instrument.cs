using UnityEngine;

namespace Tools.Base_Tool 
{
    [RequireComponent(typeof(MeshRenderer), typeof(Collider))]
    public class Instrument : MonoBehaviour {
        protected bool isActiveTool = false;
        protected bool isOcupied = false;
        private Renderer meshRenderer;
        private Collider instrumentCollider;

        protected virtual void Awake()
        {
            // Get the renderer and collider components
            meshRenderer = GetComponent<Renderer>();
            instrumentCollider = GetComponent<Collider>();

            // Hide the instrument by default
            meshRenderer.enabled = false;
            instrumentCollider.enabled = false;
        }

        public virtual void Toggle() {
            meshRenderer.enabled = !meshRenderer.enabled;
            instrumentCollider.enabled = !instrumentCollider.enabled;
        }
        
        // Set the instrument as the active tool
        public void SetActiveTool(bool active)
        {
            isActiveTool = active;
            
        }
        
        public virtual void Activate()
        {
            isActiveTool = true;
        }

        
        public virtual void Deactivate()
        {
            isActiveTool = false;
        }
    }
}