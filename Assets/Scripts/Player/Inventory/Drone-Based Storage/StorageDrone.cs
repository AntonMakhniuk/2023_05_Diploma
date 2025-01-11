using System.Collections.Generic;
using Miscellaneous.Raycast_Pathfinding;
using UnityEngine;

namespace Player.Inventory.Drone_Based_Storage
{
    public class StorageDrone : TargetFollower
    {
        [HideInInspector]
        public DroneDeliveryStorage parentStorage;

        private Dictionary<StorageDroneState, BaseStorageDroneState> _stateDictionary = new();
        private BaseStorageDroneState _currentState;
        
        private void Awake()
        {
            _stateDictionary = new Dictionary<StorageDroneState, BaseStorageDroneState>
            {
                { StorageDroneState.Idle, new IdleState(this) },
                { StorageDroneState.Approaching, new ApproachingState(this) },
                { StorageDroneState.Attracting, new AttractingState(this) },
                { StorageDroneState.Delivering, new DeliveringState(this) }
            };
            
            SetState(StorageDroneState.Idle);
        }

        public void Setup(DroneDeliveryStorage storage)
        {
            parentStorage = storage;
        }
        
        public void SetState(StorageDroneState newState)
        {
            _currentState?.Exit();
            _currentState = _stateDictionary[newState];
            _currentState.Enter();
        }
        
        public void AssignCollectable(Collectable collectable)
        {
            throw new System.NotImplementedException();
        }
    }
}