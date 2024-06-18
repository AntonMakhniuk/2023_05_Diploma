using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UI.Systems;
using UnityEngine;
using Wagons.Systems;
using Wagons.Wagon_Types;

namespace UI.Views.Overworld.General.Wagons
{
    public class WagonUIManager : MonoBehaviour, IUIElement
    {
        public static WagonUIManager Instance;
        
        [SerializedDictionary("Wagon Type", "UI Prefab")] [SerializeField]
        private SerializedDictionary<WagonType, GameObject> prefabAssociations;
        
        [SerializeField] private WagonUIElement shipUI;
        [SerializeField] private float lengthBetweenWagons;

        private readonly List<GameObject> _wagonInstances = new();
        private readonly List<WagonUIElement> _wagonsUI = new();

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
        
        public void Initialize()
        {
            GenerateWagonUIElements();
        }

        public void UpdateElement()
        {
            WagonManager.Instance.StartModification();
            
            UpdateWagonUIElements();
        }

        public void CloseElement()
        {
            WagonManager.Instance.EndModification();
        }
        
        //Test Methods
        public void CreateStorageWagon()
        {
            WagonManager.Instance.CreateWagon(WagonType.Storage);
            
            UpdateWagonUIElements();
        }

        public void CreateGeneralWagon()
        {
            WagonManager.Instance.CreateWagon(WagonType.General);
            
            UpdateWagonUIElements();
        }

        public void DetachWagonsFromShip()
        {
            //  WagonManager.Instance.DisconnectWagonsFromShip();
            
            UpdateWagonUIElements();
        }

        private void GenerateWagonUIElements()
        {
            foreach (var wagon in WagonManager.Instance.GetAllAttachedWagons())
            {
                var wagonPrefab = prefabAssociations[wagon.GetWagonType()];
                
                var pos = _wagonsUI.Count == 0
                    ? shipUI.backJointTransform.position
                    : _wagonsUI[^1].backJointTransform.position;
                
                pos.x += lengthBetweenWagons;

                var newWagon =
                    Instantiate(wagonPrefab, Vector3.zero, shipUI.transform.rotation, transform);
                
                newWagon.SetActive(false);
                
                var wagonUI = newWagon.GetComponent<WagonUIElement>();
                
                pos.x -= wagonUI.frontJointTransform.transform.position.x;

                newWagon.transform.position = pos;
                
                newWagon.SetActive(true);
                
                _wagonInstances.Add(newWagon);
                _wagonsUI.Add(wagonUI);
            }
        }

        private void UpdateWagonUIElements()
        {
            foreach (var wagonInstance in _wagonInstances)
            {
                DestroyImmediate(wagonInstance);
            }
            
            _wagonInstances.Clear();
            _wagonsUI.Clear();
            
            GenerateWagonUIElements();
        }
    }
}