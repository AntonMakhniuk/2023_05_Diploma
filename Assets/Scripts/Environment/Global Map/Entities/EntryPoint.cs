using Environment.Global_Map.Systems;
using Miscellaneous.Scene_Management;
using UnityEngine;
using UnityEngine.Events;

namespace Environment.Global_Map.Entities
{
    [System.Serializable]
    public class EntryPointEvent : UnityEvent<EntryPoint> { }
    
    public class EntryPoint : MonoBehaviour
    {
        public SceneTypeContainer sceneContainer;
        
        void Start()
        {
            GlobalMapManager.Instance.AddEntryPoint(this);
        }

        public EntryPointEvent onPlayerShipEnteredEntryPointProximity;
        public EntryPointEvent onPlayerShipLeftEntryPointProximity;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                onPlayerShipEnteredEntryPointProximity?.Invoke(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                onPlayerShipLeftEntryPointProximity?.Invoke(this);
            }
        }
    }
}
