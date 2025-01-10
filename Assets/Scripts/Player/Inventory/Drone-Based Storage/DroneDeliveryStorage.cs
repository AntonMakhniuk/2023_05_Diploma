using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Player.Ship.Tools.Marker;
using UnityEngine;

namespace Player.Inventory.Drone_Based_Storage
{
    public class DroneDeliveryStorage : StorageComponent
    {
        [Foldout("Drone Storage Data")] [SerializeField]
        private GameObject spawnPoint;
        
        [Foldout("Drone Data")] [SerializeField] [MinValue(1)]
        private int maxDroneCapacity;
        [Foldout("Drone Data")] [SerializeField]
        private GameObject dronePrefab;
        
        public int CurrentlyAssignedCollectableCount => _currentlyAssignedCollectables.Count;
        
        private readonly List<Collectable> _currentlyAssignedCollectables = new();
        private readonly Dictionary<GameObject, StorageDrone> _dronesObjectComponent = new();

        private void Awake()
        {
            for (var i = 0; i < maxDroneCapacity; i++)
            {
                var droneObject = 
                    Instantiate(dronePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation, transform);
                
                droneObject.SetActive(false);
                
                _dronesObjectComponent.Add(droneObject, droneObject.GetComponent<StorageDrone>());
            }
        }

        public void AssignCollectable(Collectable collectable)
        {
            var droneObject = _dronesObjectComponent.Keys.FirstOrDefault(drone => !drone.activeSelf);

            if (droneObject == null)
            {
                
            }
            else
            {
                droneObject.SetActive(true);
                _dronesObjectComponent[droneObject].AssignCollectable(collectable);
                _currentlyAssignedCollectables.Add(collectable);
            }
        }
    }
}