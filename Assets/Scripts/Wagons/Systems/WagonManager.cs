using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using JetBrains.Annotations;
using Miscellaneous;
using UnityEngine;
using Wagons.Miscellaneous;
using Wagons.Wagon_Types;

namespace Wagons.Systems
{
    public class WagonManager : MonoBehaviour
    {
        public static WagonManager Instance;
        
        public float wagonSpawnDistance;
        
        [SerializedDictionary("Wagon Type", "Associated Prefab")]
        [SerializeField] private SerializedDictionary<WagonType, GameObject> wagonTypePrefabAssociations;
        [SerializeField] private WagonPlayerShip shipWagonComponent;
        
        private List<WagonChain> _allChains = new();
        private WagonChain _attachedChain = new();
        private List<IWagon> _attachedWagons = new();
        private WagonChain _attachedChainBackup = new();
        private Transform _shipTransform;
        private bool _modificationAllowed;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _attachedChain = new WagonChain();
            _attachedWagons = _attachedChain.AttachedWagons;
            
            _shipTransform = shipWagonComponent.transform;
        }

        public List<IWagon> GetAllAttachedWagons()
        {
            return _attachedWagons;
        }
        
        public List<T> GetAllAttachedWagonsOfType<T>() where T : IWagon
        {
            return _attachedWagons.OfType<T>().ToList();
        }
        
        public IWagon GetWagonAtIndex(int index)
        {
            return _attachedWagons[index];
        }

        public delegate void WagonListChangedHandler(IWagon[] wagons, WagonOperationType operationType);

        public event WagonListChangedHandler OnWagonListChanged;

        public void StartModification()
        {
            _modificationAllowed = true;
            _attachedChainBackup = _attachedChain;

            DisableWagons(null);
        }

        public void EndModification()
        {
            UpdateWagonConnections();
            EnableWagons(null);
            
            _modificationAllowed = false;
        }

        private void DisableWagons([CanBeNull] WagonChain optionalChain)
        {
            if (optionalChain != null)
            {
                foreach (var wagon in optionalChain.AttachedWagons)
                {
                    wagon.GetWagon().gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (var wagon in _attachedWagons)
                {
                    wagon.GetWagon().gameObject.SetActive(false);
                }    
            }
        }
        
        private void EnableWagons([CanBeNull] WagonChain optionalChain)
        {
            if (optionalChain != null)
            {
                foreach (var wagon in optionalChain.AttachedWagons)
                {
                    wagon.GetWagon().gameObject.SetActive(true);
                }
            
                foreach (var wagon in optionalChain.AttachedWagons)
                {
                    wagon.GetWagon().backJoint.UpdateAnchors();
                }
            }
            else
            {
                foreach (var wagon in _attachedWagons)
                {
                    wagon.GetWagon().gameObject.SetActive(true);
                }

                Debug.Log(shipWagonComponent == null);
                Debug.Log(shipWagonComponent.backJoint == null);
                
                shipWagonComponent.backJoint.UpdateAnchors();
            
                foreach (var wagon in _attachedWagons)
                {
                    wagon.GetWagon().backJoint.UpdateAnchors();
                }    
            }
        }

        public void EndModificationWithoutSaving()
        {
            _attachedChain = _attachedChainBackup;
            
            foreach (var wagon in _attachedWagons)
            {
                wagon.GetWagon().gameObject.SetActive(true);
            }
            
            _modificationAllowed = false;
        }
        
        public void CreateWagon(WagonType type) 
        {
            if (!_modificationAllowed)
            {
                Debug.Log("Tried to create new wagon while modification was not allowed.");
                
                return;
            }
            
            var backJointPosition = _attachedChain.AttachedWagons.Count == 0
                ? shipWagonComponent.backJoint.GetPosition()
                : _attachedChain.AttachedWagons[^1].GetWagon().backJoint.GetPosition();
            
            GameObject newWagon = Instantiate
            (
                wagonTypePrefabAssociations[type],
                backJointPosition,
                _shipTransform.rotation,
                transform
            );
            var wagonComponent = newWagon.GetComponent<IWagon>();
            
            newWagon.transform.position -= _shipTransform.forward *
                                           (wagonComponent.GetWagon().backJoint.GetAbsDistanceFromWagonCenter()
                                            + wagonSpawnDistance);
            
            newWagon.SetActive(false);
            
            AddWagonToBack(wagonComponent);
                
            OnWagonListChanged?.Invoke(new[] { wagonComponent }, 
                WagonOperationType.Creation);
        }
        
        public void SwitchWagonPositions(IWagon firstWagon, IWagon secondWagon)
        {
            if (!_modificationAllowed)
            {
                Debug.Log("Tried to switch wagons while modification was not allowed.");
                
                return;
            }
            
            _attachedWagons.Swap(firstWagon, secondWagon);
            
            OnWagonListChanged?.Invoke(new[] { firstWagon, secondWagon }, WagonOperationType.Moving);
        }

        public void MoveWagonInChain(int oldIndex, int newIndex)
        {
            if (!_modificationAllowed)
            {
                Debug.Log("Tried to move wagon in chain while modification was not allowed.");
                
                return;
            }
            
            if (oldIndex < 0 || oldIndex >= _attachedWagons.Count 
                || newIndex < 0 || newIndex >= _attachedWagons.Count)
            {
                Debug.Log("Tried to access index outside the current chain");
                
                return;
            }

            if (oldIndex == newIndex)
            {
                return;
            }

            IWagon wagonTemp = _attachedWagons[oldIndex];

            List<IWagon> affectedWagons = new();
            
            if (oldIndex < newIndex)
            {
                for (int i = oldIndex; i <= newIndex; i++)
                {
                    affectedWagons.Add(_attachedWagons[i]);
                }
            }
            else
            {
                for (int i = newIndex; i <= oldIndex; i++)
                {
                    affectedWagons.Add(_attachedWagons[i]);
                }
            }
            
            _attachedWagons.RemoveAt(oldIndex);
            _attachedWagons.Insert(newIndex, wagonTemp);
            
            OnWagonListChanged?.Invoke(affectedWagons.ToArray(), WagonOperationType.Moving);
        }

        public void AddWagonToBack(IWagon wagon)
        {
            if (!_modificationAllowed)
            {
                Debug.Log("Tried to add wagon while modification was not allowed.");
                
                return;
            }
            
            _attachedWagons.Add(wagon);
            
            OnWagonListChanged?.Invoke(new[] { wagon }, WagonOperationType.Addition);
        }

        public void RemoveWagon(IWagon wagon)
        {
            if (!_modificationAllowed)
            {
                Debug.Log("Tried to remove wagon while modification was not allowed.");
                
                return;
            }
            
            _attachedWagons.Remove(wagon);
            
            OnWagonListChanged?.Invoke(new[] { wagon }, WagonOperationType.Removal);
        }

        public void RemoveWagon(int index)
        {
            if (index >= _attachedWagons.Count || index < 0)
            {
                Debug.Log("Tried to remove wagon at index outside the list");
                
                return;
            }
            
            var wagonToRemove = _attachedWagons.ElementAt(index);
            
            RemoveWagon(wagonToRemove);
        }

        private void UpdateWagonConnections()
        {
            if (!_modificationAllowed)
            {
                Debug.Log("Tried to update wagons while modification was not allowed.");
                
                return;
            }
            
            if (_attachedWagons.Count == 0)
            {
                return;
            }
            
            var tempChain = DisconnectWagonsFromShip();

            foreach (var wagon in tempChain.AttachedWagons)
            {
                DisconnectWagonFrom(wagon);
            }
            
            for (int i = 0; i < tempChain.AttachedWagons.Count; i++)
            {
                var backJointPosition = i == 0
                    ? shipWagonComponent.backJoint.GetPosition()
                    : tempChain.AttachedWagons[i - 1].GetWagon().backJoint.GetPosition();
                
                var wagon = tempChain.AttachedWagons[i].GetWagon();
                
                wagon.transform.position = 
                    backJointPosition - _shipTransform.forward *
                    (wagon.GetWagon().backJoint.GetAbsDistanceFromWagonCenter()
                     + wagonSpawnDistance);
            }
            
            ConnectWagonsToShip(tempChain);

            if (_attachedWagons.Count == 1)
            {
                return;
            }
            
            for (int i = 0; i < _attachedWagons.Count - 1; i++)
            {
                ConnectWagons(_attachedWagons.ElementAt(i), _attachedWagons.ElementAt(i + 1));
            }
            
            OnWagonListChanged?.Invoke(_attachedWagons.ToArray(), WagonOperationType.Update);
        }
        
        private void DisconnectWagonFrom(IWagon parentWagon)
        {
            parentWagon.GetWagon().backJoint.Disconnect();
        }

        public void HandleDisconnectWagonsInput()
        {
            if (_attachedWagons.Count == 0)
            {
                Debug.Log("There are no wagons to disconnect");

                return;
            }

            var chain = DisconnectWagonsFromShip();
            EnableWagons(chain);
        }
        
        private WagonChain DisconnectWagonsFromShip()
        {
            if (_attachedWagons.Count == 0)
            {
                return null;
            }
            
            DisconnectWagonFrom(shipWagonComponent);
            
            var newChain = new WagonChain { AttachedWagons = new List<IWagon>(_attachedWagons) };
            
            _allChains.Add(newChain);
            _attachedWagons.Clear();
            
            OnWagonListChanged?.Invoke(newChain.AttachedWagons.ToArray(), WagonOperationType.Removal);

            return newChain;
        }
        
        private void ConnectWagons(IWagon parentWagon, IWagon childWagon)
        {
            parentWagon.GetWagon().backJoint.Connect(childWagon.GetWagon());
        }

        public void ConnectWagonsToShip(WagonChain chain)
        {
            if (chain.AttachedWagons.Count == 0)
            {
                _allChains.Remove(chain);
                
                return;
            }
            
            ConnectWagons(shipWagonComponent, chain.AttachedWagons.ElementAt(0));

            _allChains.Remove(chain);
            
            _attachedWagons = chain.AttachedWagons;
            
            OnWagonListChanged?.Invoke(_attachedWagons.ToArray(), WagonOperationType.Addition);
        }
        
        public void SetDragValuesForAttachedWagons(float drag, float angularDrag)
        {
            foreach (var wagon in _attachedWagons)
            {
                wagon.SetDragValues(drag, angularDrag);
            }
        }
    }

    public enum WagonOperationType
    {
        Creation, Removal, Addition, Moving, Update
    }
}