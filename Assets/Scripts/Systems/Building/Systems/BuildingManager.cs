using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Building.Buildings.Base_Classes;
using Player;
using Player.Movement.Miscellaneous;
using Player.Ship;
using Scriptable_Object_Templates.Singletons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Building.Systems
{
    // TODO: further rework cause this is ehhhh
    public class BuildingManager : MonoBehaviour
    {
        private const float MaxBuildingDistance = 25f;
        private const float RotationSpeed = 45f;
        
        public static BuildingManager Instance;
        
        public readonly Dictionary<BuildingType, List<BuildingParent>> BuildingsByType = new();

        private IEnumerator _updateBlueprintCoroutine;
        private BuildingParent _currentParent;
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
            if (_currentParent == null)
            {
                return;
            }

            var tempAngles = _currentRotation.eulerAngles;
            tempAngles.z -= RotationSpeed * Time.deltaTime;
            _currentRotation.eulerAngles = tempAngles;
        }
        
        private void HandleRotateRight(InputAction.CallbackContext obj)
        {
            if (_currentParent == null)
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
                
                var currentObject = Instantiate(BuildingPrefabDictionary.Instance.dictionary[buildingType].prefab,
                    spawnPosition, _currentRotation, transform);
                
                _currentParent = currentObject.GetComponent<BuildingParent>();

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

                    _currentParent.gameObject.transform.position = spawnPosition;
                    _currentParent.gameObject.transform.rotation = _currentRotation;
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
            if (_currentParent == null)
            {
                return;
            }
            
            Destroy(_currentParent.gameObject);
                
            _currentParent = null;
            
            StopCoroutine(_updateBlueprintCoroutine);
        }

        private void HandlePlaceBlueprint(InputAction.CallbackContext _)
        {
            HandlePlaceBlueprint();
        }
        
        private void HandlePlaceBlueprint()
        {
            if (_currentParent == null)
            {
                return;
            }
            
            if (!BuildingsByType.TryAdd(_currentParent.buildingType, new List<BuildingParent> { _currentParent }))
            {
                BuildingsByType[_currentParent.buildingType].Add(_currentParent);
            }

            _currentParent.blueprintComponent.OnBlueprintResourcesFulfilled += HandleConstructBlueprint;
            _currentParent = null;
            
            StopCoroutine(_updateBlueprintCoroutine);
        }

        private void HandleConstructBlueprint(object sender, BaseBlueprint blueprint)
        {
            BuildingsByType.Values
                .SelectMany(i => i)
                .Single(b => b.blueprintComponent == blueprint)
                .SetState(BuildingState.Constructed);
        }
        
        private void OnDestroy()
        {
            foreach (var building in BuildingsByType.Values)
            {
                building.ForEach(b => b.blueprintComponent
                    .OnBlueprintResourcesFulfilled -= HandleConstructBlueprint);
            }
            
            PlayerActions.InputActions.Building.PlaceDownBlueprint.performed -= HandlePlaceBlueprint;
            PlayerActions.InputActions.Building.ClearBlueprint.performed -= HandleClearBlueprint;
            PlayerActions.InputActions.Building.RotateBlueprintLeft.performed -= HandleRotateLeft;
            PlayerActions.InputActions.Building.RotateBlueprintRight.performed -= HandleRotateRight;
        }
    }
}
