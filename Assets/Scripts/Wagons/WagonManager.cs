using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Miscellaneous;
using UnityEngine;
using Wagons.Wagon_Types;

namespace Wagons
{
    public class WagonManager : MonoBehaviour
    {
        public static WagonManager Instance;
        
        [SerializedDictionary("Wagon Type", "Associated Prefab")]
        [SerializeField] private SerializedDictionary<WagonType, GameObject> wagonTypePrefabAssociations;
        [SerializeField] private float wagonSpawnDistance;

        private List<WagonChain> _allChains = new();
        private WagonChain _attachedChain = new();
        private List<IWagon> _attachedWagons = new();
        private WagonChain _attachedChainBackup = new();
        private PlayerShipComponent _shipComponent;
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
            _shipComponent = GetComponentInParent<PlayerShipComponent>();

            _attachedChain = new WagonChain();
            _attachedWagons = _attachedChain.AttachedWagons;
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

            foreach (var wagon in _attachedWagons)
            {
                wagon.GetWagon().gameObject.SetActive(false);
            }
        }

        public void EndModification()
        {
            UpdateWagonConnections();

            foreach (var wagon in _attachedWagons)
            {
                wagon.GetWagon().gameObject.SetActive(true);
            }
            
            _modificationAllowed = false;
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
            
            Vector3 backJointPosition = _shipComponent.backJoint.transform.position;
            Vector3 wagonPosition = 
                backJointPosition - _shipComponent.transform.forward * (_attachedWagons.Count + 1) * wagonSpawnDistance;

            GameObject newWagon = null;

            switch (type)
            {
                case WagonType.General:
                {
                    newWagon = Instantiate
                    (
                        wagonTypePrefabAssociations[WagonType.General],
                        wagonPosition,
                        _shipComponent.transform.rotation
                    );
                    break;
                }
                case WagonType.Storage:
                {
                    newWagon = Instantiate
                    (
                        wagonTypePrefabAssociations[WagonType.Storage],
                        wagonPosition,
                        _shipComponent.transform.rotation
                    );
                    break;
                }
            }

            if (newWagon == null)
            {
                return;
            }
            
            AddWagonToBack(newWagon.GetComponent<IWagon>());
                
            OnWagonListChanged?.Invoke(new[] { newWagon.GetComponent<IWagon>() }, 
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
                wagon.GetWagon().backJoint.Disconnect();
                wagon.GetWagon().frontJoint.Disconnect();
            }
            
            Vector3 backJointPosition = _shipComponent.backJoint.transform.position;
            
            for (int i = 0; i < tempChain.AttachedWagons.Count; i++)
            {
                var wagon = tempChain.AttachedWagons[i].GetWagon();
                
                Vector3 wagonPosition = 
                    backJointPosition - _shipComponent.transform.forward * (i + 1) * wagonSpawnDistance;
                
                wagon.transform.position = wagonPosition;
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
        
        private void DisconnectWagons(IWagon parentWagon, IWagon childWagon)
        {
            parentWagon.GetWagon().backJoint.Disconnect();
            childWagon.GetWagon().frontJoint.Disconnect();
        }

        public WagonChain DisconnectWagonsFromShip()
        {
            if (_attachedWagons.Count == 0)
            {
                return null;
            }
            
            _shipComponent.backJoint.Disconnect();
            _attachedWagons.ElementAt(0).GetWagon().frontJoint.Disconnect();

            var newChain = new WagonChain { AttachedWagons = _attachedWagons };
            
            _allChains.Add(newChain);
            _attachedWagons.Clear();
            
            OnWagonListChanged?.Invoke(newChain.AttachedWagons.ToArray(), WagonOperationType.Removal);

            return newChain;
        }
        
        private void ConnectWagons(IWagon parentWagon, IWagon childWagon)
        {
            parentWagon.GetWagon().backJoint.Connect(childWagon.GetWagon().frontJoint);
            childWagon.GetWagon().frontJoint.Connect(parentWagon.GetWagon().backJoint);
        }

        public void ConnectWagonsToShip(WagonChain chain)
        {
            if (chain.AttachedWagons.Count == 0)
            {
                _allChains.Remove(chain);
                
                return;
            }
            
            _shipComponent.backJoint.Connect(chain.AttachedWagons.ElementAt(0).GetWagon().frontJoint);
            chain.AttachedWagons.ElementAt(0).GetWagon().frontJoint.Connect(_shipComponent.backJoint);

            _allChains.Remove(chain);
            _attachedWagons = chain.AttachedWagons;
            
            OnWagonListChanged?.Invoke(_attachedWagons.ToArray(), WagonOperationType.Addition);
        }
    }

    public enum WagonOperationType
    {
        Creation, Removal, Addition, Moving, Update
    }
}