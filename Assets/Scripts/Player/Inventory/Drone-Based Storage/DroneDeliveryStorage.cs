using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Player.Inventory.Drone_Based_Storage
{
    public class DroneDeliveryStorage : StorageComponent
    {
        [Foldout("Drone Storage Data")] [SerializeField]
        private List<Transform> droneSpawnPoints;
        
        [Foldout("Drone Data")] [SerializeField] [MinValue(1)]
        private int maxDroneCapacity;
        [Foldout("Drone Data")] [SerializeField]
        private GameObject dronePrefab;
        
        public int CurrentlyAssignedCollectableCount => _currentlyAssignedCollectables.Count;
        
        private readonly List<Collectable> _currentlyAssignedCollectables = new();
        private readonly Dictionary<GameObject, StorageDrone> _dronesObjectComponent = new();
        private readonly Dictionary<StorageDrone, Transform> _droneSpawnPoints = new();

        private void Awake()
        {
            for (var i = 0; i < maxDroneCapacity; i++)
            {
                var spawnPoint = droneSpawnPoints
                    .First(sp => !_droneSpawnPoints.Values.Contains(sp));
                
                var droneObject = 
                    Instantiate(dronePrefab, spawnPoint.position, spawnPoint.rotation, transform);
                
                droneObject.SetActive(false);

                var droneComponent = droneObject.GetComponent<StorageDrone>();
                
                droneComponent.Setup(this);
                
                _dronesObjectComponent.Add(droneObject, droneComponent);
                _droneSpawnPoints.Add(droneComponent, spawnPoint);
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

        public Transform GetSpawnPointByDrone(StorageDrone drone)
        {
            return _droneSpawnPoints[drone];
        }
    }
}