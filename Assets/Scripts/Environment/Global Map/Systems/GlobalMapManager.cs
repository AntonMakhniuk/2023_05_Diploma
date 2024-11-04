using System;
using System.Collections.Generic;
using Environment.Global_Map.Entities;
using UnityEngine;

namespace Environment.Global_Map.Systems
{
    public class GlobalMapManager : MonoBehaviour
    {
        public static GlobalMapManager Instance;
        
        private readonly List<EntryPoint> _entryPoints = new();

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

        public void AddEntryPoint(EntryPoint point)
        {
            if (_entryPoints.Contains(point))
            {
                return;
            }
            
            _entryPoints.Add(point);

            point.onPlayerShipEnteredEntryPointProximity.AddListener(RelayPlayerShipEnteredEntryPointProximity);
            point.onPlayerShipLeftEntryPointProximity.AddListener(RelayPlayerShipLeftEntryPointProximity);
        }

        public event EventHandler<EntryPoint> OnRelayPlayerShipEnteredEntryPointProximity, 
            OnRelayPlayerShipLeftEntryPointProximity; 
        
        private void RelayPlayerShipEnteredEntryPointProximity(EntryPoint entryPoint)
        {
            OnRelayPlayerShipEnteredEntryPointProximity?.Invoke(this, entryPoint);
        }

        private void RelayPlayerShipLeftEntryPointProximity(EntryPoint entryPoint)
        {
            OnRelayPlayerShipLeftEntryPointProximity?.Invoke(this, entryPoint);
        }

        private void OnDestroy()
        {
            foreach (var point in _entryPoints)
            {
                point.onPlayerShipEnteredEntryPointProximity.RemoveListener(RelayPlayerShipEnteredEntryPointProximity);
                point.onPlayerShipLeftEntryPointProximity.RemoveListener(RelayPlayerShipLeftEntryPointProximity);
            }
        }
    }
}