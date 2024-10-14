using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Miscellaneous.Scene_Management
{
    public class InputStateManager : MonoBehaviour
    {
        public static InputStateManager Instance;
        
        public List<SceneType> scenesWithNoInput = new();

        private CinemachineBrain _mainCameraBrain;

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
            SceneManager.sceneLoaded += ChangeInput;
            SceneManager.sceneLoaded += UpdateMainCamera;
            
            ChangeInput(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            UpdateMainCamera(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        private void ChangeInput(Scene scene, LoadSceneMode _)
        {
            var scenes = LevelManager.Instance.scenesDict
                .Where(kvp => scenesWithNoInput
                    .Contains(kvp.Key))
                .ToList();

            var isSceneNoInput = scenes.Any(kvp => kvp.Value.Any(s => s == scene.name));

            if (isSceneNoInput)
            {
                PlayerActions.InputActions.Disable();
            }
            else
            {
                PlayerActions.InputActions.Enable();
            }
        }
        
        private void UpdateMainCamera(Scene _, LoadSceneMode __)
        {
            if (Camera.main != null)
            {
                _mainCameraBrain = Camera.main.GetComponent<CinemachineBrain>();
            }
        }

        public void SetCameraMovementState(bool newState)
        {
            if (_mainCameraBrain == null)
            {
                return;
            }

            if (_mainCameraBrain.ActiveVirtualCamera.VirtualCameraGameObject
                .TryGetComponent<CinemachineInputProvider>(out var provider))
            {
                provider.enabled = newState;
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= ChangeInput;
            SceneManager.sceneLoaded -= UpdateMainCamera;
        }
    }
}