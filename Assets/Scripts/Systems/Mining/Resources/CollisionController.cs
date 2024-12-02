using UnityEngine;

namespace Systems.Mining.Resources
{
    public class CollisionController : MonoBehaviour
    {
        [SerializeField] private Collider colliderBase;
        [SerializeField] private Collider colliderTrigger;
        
        private int _objectsInsideCount;

        private void Awake()
        {
            colliderBase.excludeLayers = LayerMask.GetMask("Resource", "ResourceNode");
        }

        private void OnTriggerEnter(Collider other)
        {
            _objectsInsideCount++;
        }

        private void OnTriggerExit(Collider other)
        {
            _objectsInsideCount--;
            
            if (_objectsInsideCount > 0)
            {
                return;
            }

            colliderBase.excludeLayers = new LayerMask();
            colliderTrigger.enabled = false;
        }
    }
}
