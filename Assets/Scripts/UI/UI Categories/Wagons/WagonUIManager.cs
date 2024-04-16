using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UI.Systems;
using UnityEngine;
using Wagons;
using Wagons.Wagon_Types;

namespace UI.UI_Categories.Wagons
{
    public class WagonUIManager : MonoBehaviour, IUIElement
    {
        [SerializedDictionary("Wagon Type", "UI Prefab")] [SerializeField]
        private SerializedDictionary<WagonType, GameObject> prefabAssociations;

        [SerializeField] private WagonUIElement shipUI;
        [SerializeField] private float lengthBetweenWagons;

        private List<GameObject> _wagonInstances = new();
        private List<WagonUIElement> _wagonsUI = new();

        public void Initialize()
        {
            PanelManager.Instance.OnPanelOpened += HandlePanelOpen;
            PanelManager.Instance.OnPanelClosed += HandlePanelClose;
            
            GenerateWagonUIElements();
        }

        public void UpdateElement()
        {
            UpdateWagonUIElements();
        }

        private void HandlePanelOpen(PanelType panelType)
        {
            if (panelType != PanelType.Wagons)
            {
                return;
            }
            
            WagonManager.Instance.StartModification();
        }

        private void HandlePanelClose(PanelType panelType)
        {
            if (panelType != PanelType.Wagons)
            {
                return;
            }
            
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
            WagonManager.Instance.DisconnectWagonsFromShip();
            
            UpdateWagonUIElements();
        }

        private void GenerateWagonUIElements()
        {
            foreach (var wagon in WagonManager.Instance.GetAllAttachedWagons())
            {
                var wagonPrefab = prefabAssociations[wagon.GetWagonType()];
                var wagonUI = wagonPrefab.GetComponent<WagonUIElement>();
                
                var pos = _wagonsUI.Count == 0
                    ? shipUI.backJointTransform.position
                    : _wagonsUI[^1].backJointTransform.position;
                pos.x += lengthBetweenWagons;
                pos.x -= wagonUI.frontJointTransform.transform.position.x;

                var newWagon =
                    Instantiate(wagonPrefab, pos, shipUI.transform.rotation, transform);

                _wagonInstances.Add(newWagon);
                _wagonsUI.Add(wagonUI);
            }
        }

        private void UpdateWagonUIElements()
        {
            // TODO: look into object pooling

            foreach (var wagonInstance in _wagonInstances)
            {
                DestroyImmediate(wagonInstance);
            }
            
            _wagonInstances.Clear();
            _wagonsUI.Clear();
            
            GenerateWagonUIElements();
        }

        private void OnDestroy()
        {
            PanelManager.Instance.OnPanelOpened -= HandlePanelOpen;
            PanelManager.Instance.OnPanelClosed -= HandlePanelClose;
        }
    }
}