using System;
using System.Collections.Generic;
using UnityEngine;
using Wagons;

namespace UI.Wagons
{
    public class WagonUIManager : MonoBehaviour
    {
        [SerializeField] private List<PrefabsAssociation> prefabAssociations;

        private WagonManager _wagonManager;

        private void Start()
        {
            
        }

        public void UpdateWagons()
        {
            
        }
    }

    [Serializable]
    public class PrefabsAssociation
    {
        public GameObject prefabA, prefabB;
    }
}