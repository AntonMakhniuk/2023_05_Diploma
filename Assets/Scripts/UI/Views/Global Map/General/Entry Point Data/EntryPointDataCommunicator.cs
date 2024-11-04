using System;
using Environment.Global_Map.Entities;
using Environment.Global_Map.Systems;
using UI.Systems.Panels;
using UnityEngine;

namespace UI.Views.Global_Map.General.Entry_Point_Data
{
    public class EntryPointDataCommunicator : MonoBehaviour
    {
        [SerializeField] private UIPanel entryPointUIPanel;
        [SerializeField] private EntryPointUIManager entryPointUIManager;
        
        void Start()
        {
            GlobalMapManager.Instance.OnRelayPlayerShipEnteredEntryPointProximity += ShowEntryPointData;
            GlobalMapManager.Instance.OnRelayPlayerShipLeftEntryPointProximity += HideEntryPointData;
        }

        private void ShowEntryPointData(object sender, EntryPoint entryPoint)
        {
            Debug.Log(1);
            
            PanelManager.ToggleParentPanel(entryPointUIPanel);
            
            entryPointUIManager.UpdateEntryPointData(entryPoint);
        }

        private void HideEntryPointData(object sender, EntryPoint _)
        {
            Debug.Log(2);
            
            PanelManager.ToggleParentPanel(entryPointUIPanel);
        }

        private void OnDestroy()
        {
            GlobalMapManager.Instance.OnRelayPlayerShipEnteredEntryPointProximity -= ShowEntryPointData;
            GlobalMapManager.Instance.OnRelayPlayerShipLeftEntryPointProximity -= HideEntryPointData;
        }
    }
}
