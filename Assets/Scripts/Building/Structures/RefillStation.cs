using System.Collections;
using Building.Systems;
using Player.Movement;
using Player.Ship;
using Resource_Nodes.Gas_Cloud;
using Scriptable_Object_Templates.Resources;
using UnityEngine;

namespace Building.Structures
{
    public class RefillStation : BuildingObject
    {
        // If the distance between the refill station and closest gas field center is less than this value,
        // it is treated as being centered on the gas field, so the refill efficiency is 100%
        private const float ProximityThreshold = 3f;
        
        // Refill rate, i.e. how fast it will refill fuel on its own
        // if built in close proximity to a fuel gas field
        private const float MinRefillRate = 0;
        private const float MaxRefillRate = 0.1f;
        
        // Transfer rate, i.e. how fast it will refuel a docked ship or receive fuel from it,
        // measured in units per second
        private const float TransferRate = 3f;
        
        private const float MaxFuelCapacity = 100f;

        [SerializeField] private SphereCollider refillArea;

        private float _refillRate = MinRefillRate;
        private float _storedFuelLevel;
        private IEnumerator _refuelCoroutine, _storeCoroutine;
        private bool _isDocked;

        private void Start()
        {
            _refillRate = CalculateRefillRate();

            if (_refillRate > 0)
            {
                InvokeRepeating(nameof(Refill), 0, 1);
            }

            _refuelCoroutine = RefuelPlayerCoroutine();
            _storeCoroutine = StoreFuelCoroutine();
        }

        private float CalculateRefillRate()
        {
            var refillRate = MinRefillRate;

            foreach (var overlappingCollider in Physics.OverlapSphere(refillArea.center, refillArea.radius))
            {
                if (overlappingCollider == refillArea)
                {
                    continue;
                }

                if (refillArea.ClosestPoint(overlappingCollider.transform.position)
                    != overlappingCollider.transform.position)
                {
                    continue;
                }

                if (!overlappingCollider.gameObject.TryGetComponent<GasCloud>(out var gasCloudNode))
                {
                    continue;
                }

                if (gasCloudNode.associatedResource.resourceType != ResourceType.FuelGas)
                {
                    continue;
                }

                var distanceFromCloud
                    = Vector3.Distance(transform.position, overlappingCollider.transform.position);

                if (distanceFromCloud <= ProximityThreshold)
                {
                    _refillRate = MaxRefillRate;

                    break;
                }

                var currentRefillRate = MaxRefillRate * distanceFromCloud / refillArea.radius;

                if (currentRefillRate > refillRate)
                {
                    refillRate = currentRefillRate;
                }
            }

            return refillRate;
        }

        private void Refill()
        {
            _storedFuelLevel = Mathf.Min(_storedFuelLevel + _refillRate, MaxFuelCapacity);
        }
        
        public void DockToStation(RefillStationAction action)
        {
            if (_isDocked)
            {
                return;
            }

            _isDocked = true;

            StartCoroutine(action == RefillStationAction.Refuel ? _refuelCoroutine : _storeCoroutine);
        }

        public void UndockFromStation()
        {
            if (!_isDocked)
            {
                return;
            }

            _isDocked = false;
            
            StopAllCoroutines();
        }
        
        private IEnumerator RefuelPlayerCoroutine()
        {
            var fuelSystem = PlayerShip.Instance.GetComponent<FuelSystem>();
            var refuelAmount = TransferRate * Time.deltaTime;
            
            fuelSystem.AddFuel(refuelAmount);
            
            _storedFuelLevel -= refuelAmount;

            yield return null;
        }

        private IEnumerator StoreFuelCoroutine()
        {
            var fuelSystem = PlayerShip.Instance.GetComponent<FuelSystem>();
            var refuelAmount = Mathf.Min(fuelSystem.CurrentFuelLevel, TransferRate * Time.deltaTime);
            
            fuelSystem.AddFuel(refuelAmount);
            
            _storedFuelLevel -= refuelAmount;

            yield return null;
        }
    }

    public enum RefillStationAction
    {
        Refuel, Store
    }
}
