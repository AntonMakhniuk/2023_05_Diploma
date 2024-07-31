using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Player;
using Player.Ship;
using Scriptable_Object_Templates.Building;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Building.Systems
{
    public class BuildingManager : MonoBehaviour
    {
        public static BuildingManager Instance;

        [SerializedDictionary("Building Object", "Building Prefab")]
        public SerializedDictionary<BuildingType, BuildingData> buildingDataByType = new();
        
        public readonly Dictionary<BuildingType, List<BuildingObject>> InstancesByType = new();

        [SerializeField] private float buildingOffset;
        
        private BuildingType _currentType;
        private GameObject _currentBlueprint;

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
            //TODO: make this less messy later, Im fucking sleepy
            PlayerActions.InputActions.UI.PlaceBlueprint.performed += PlaceDownBuilding;
            PlayerActions.InputActions.UI.RemoveBlueprint.performed += RemoveCurrentBlueprint;
        }

        public static void InstantiateBuildingBlueprint(BuildingTypeContainer container)
        {
            InstantiateBuildingBlueprint(container.buildingType);
        }
    
        public static void InstantiateBuildingBlueprint(BuildingType buildingType)
        {
            var shipTransform = PlayerShip.Instance.transform;

            var spawnPosition = shipTransform.position + shipTransform.forward * Instance.buildingOffset;

            Instance._currentType = buildingType;
            Instance._currentBlueprint = Instantiate(Instance.buildingDataByType[buildingType].prefab,
                spawnPosition, shipTransform.rotation, shipTransform);
        }

        private static void RemoveCurrentBlueprint(InputAction.CallbackContext callbackContext)
        {
            RemoveCurrentBlueprint();
        }
        
        private static void RemoveCurrentBlueprint()
        {
            if (Instance._currentBlueprint == null)
            {
                return;
            }
            
            Destroy(Instance._currentBlueprint);
                
            Instance._currentBlueprint = null;
        }

        private static void PlaceDownBuilding(InputAction.CallbackContext obj)
        {
            PlaceDownBuilding();
        }
        
        private static void PlaceDownBuilding()
        {
            if (Instance._currentBlueprint == null)
            {
                return;
            }
            
            if (!Instance.InstancesByType.TryAdd(Instance._currentType,
                    new List<BuildingObject> { Instance._currentBlueprint.GetComponent<BuildingObject>() }))
            {
                Instance.InstancesByType[Instance._currentType]
                    .Add(Instance._currentBlueprint.GetComponent<BuildingObject>());
            }
            
            Instance._currentBlueprint.transform.parent = Instance.transform;
            
            Instance._currentBlueprint = null;
        }
    }
}
