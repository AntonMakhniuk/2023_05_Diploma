using System.Collections;
using System.Collections.Generic;
using Building.Structures;
using Player;
using Player.Ship;
using Scriptable_Object_Templates.Singletons.Dictionaries;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Building.Systems
{
    public class BuildingManager : MonoBehaviour
    {
        private const float MaxBuildingDistance = 25f;
        private const float RotationSpeed = 45f;
        
        public static BuildingManager Instance;
        
        public readonly Dictionary<BuildingType, List<BaseBuilding>> ConstructedByType = new();
        public readonly Dictionary<BuildingType, List<BaseBuilding>> BlueprintsByType = new();

        private IEnumerator _updateBlueprintCoroutine;
        private BuildingType _currentType;
        private GameObject _currentObject;
        private BaseBuilding _currentBuilding;
        private Quaternion _currentRotation;
        
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
            PlayerActions.InputActions.Building.PlaceDownBlueprint.performed += HandlePlaceBlueprint;
            PlayerActions.InputActions.Building.ClearBlueprint.performed += HandleClearBlueprint;
            PlayerActions.InputActions.Building.RotateBlueprintLeft.performed += HandleRotateLeft;
            PlayerActions.InputActions.Building.RotateBlueprintRight.performed += HandleRotateRight;
            
            _updateBlueprintCoroutine = UpdateBlueprintCoroutine();
        }

        private void HandleRotateLeft(InputAction.CallbackContext _)
        {
            if (_currentObject == null)
            {
                return;
            }

            var tempAngles = _currentRotation.eulerAngles;
            tempAngles.z -= RotationSpeed * Time.deltaTime;
            _currentRotation.eulerAngles = tempAngles;
        }
        
        private void HandleRotateRight(InputAction.CallbackContext obj)
        {
            if (_currentObject == null)
            {
                return;
            }

            var tempAngles = _currentRotation.eulerAngles;
            tempAngles.z -= RotationSpeed * Time.deltaTime;
            _currentRotation.eulerAngles = tempAngles;
        }

        public void InstantiateBuildingBlueprint(BuildingTypeContainer container)
        {
            InstantiateBuildingBlueprint(container.buildingType);
        }
    
        public void InstantiateBuildingBlueprint(BuildingType buildingType)
        {
            var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            
            if (groundPlane.Raycast(ray, out var intersect))
            {
                var spawnPosition = ray.GetPoint(intersect);
                
                if (Vector3.Distance(Camera.main.transform.position, spawnPosition) > MaxBuildingDistance)
                {
                    var direction = (spawnPosition - Camera.main.transform.position).normalized;
                    spawnPosition = Camera.main.transform.position + direction * MaxBuildingDistance;
                    spawnPosition.y = 0;    
                }

                _currentRotation = PlayerShip.Instance.transform.rotation;
                
                _currentObject = Instantiate(BuildingTypeDataDictionary.Instance.dictionary[buildingType].prefab,
                    spawnPosition, _currentRotation, transform);
                
                _currentBuilding = _currentObject.GetComponent<BaseBuilding>();
                _currentBuilding.blueprint.SetActive(true);
                _currentBuilding.finished.SetActive(false);

                StartCoroutine(_updateBlueprintCoroutine);
            }
        }

        private IEnumerator UpdateBlueprintCoroutine()
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);

            while (true)
            {
                var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                
                if (groundPlane.Raycast(ray, out var intersect))
                {
                    var spawnPosition = ray.GetPoint(intersect);
                
                    if (Vector3.Distance(Camera.main.transform.position, spawnPosition) > MaxBuildingDistance)
                    {
                        var direction = (spawnPosition - Camera.main.transform.position).normalized;
                        spawnPosition = Camera.main.transform.position + direction * MaxBuildingDistance;
                        spawnPosition.y = 0;    
                    }

                    _currentObject.transform.position = spawnPosition;
                    _currentObject.transform.rotation = _currentRotation;
                }
                
                yield return null;
            }
        }
        
        private void HandleClearBlueprint(InputAction.CallbackContext _)
        {
            HandleClearBlueprint();
        }
        
        private void HandleClearBlueprint()
        {
            if (_currentObject == null)
            {
                return;
            }
            
            Destroy(_currentObject);
                
            _currentObject = null;
            
            StopCoroutine(_updateBlueprintCoroutine);
        }

        private void HandlePlaceBlueprint(InputAction.CallbackContext _)
        {
            HandlePlaceBlueprint();
        }
        
        private void HandlePlaceBlueprint()
        {
            if (_currentObject == null)
            {
                return;
            }
            
            if (!BlueprintsByType.TryAdd(_currentType,
                    new List<BaseBuilding> { _currentObject.GetComponent<BaseBuilding>() }))
            {
                BlueprintsByType[_currentType]
                    .Add(_currentObject.GetComponent<BaseBuilding>());
            }
            
            _currentObject = null;
            
            StopCoroutine(_updateBlueprintCoroutine);
        }

        private void OnDestroy()
        {
            PlayerActions.InputActions.Building.PlaceDownBlueprint.performed -= HandlePlaceBlueprint;
            PlayerActions.InputActions.Building.ClearBlueprint.performed -= HandleClearBlueprint;
        }
    }
}
