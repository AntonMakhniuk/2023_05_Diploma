using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Miscellaneous
{
    public class InputBlocker : MonoBehaviour
    {
        public List<SceneType> scenesNoInput = new();

        private void Start()
        {
            SceneManager.sceneLoaded += (scene, _) => ChangeInput(scene);
        }

        private void ChangeInput(Scene scene)
        {
            var scenes = LevelManager.Instance.scenesDict
                .Where(kvp => scenesNoInput
                    .Contains(kvp.Key))
                .ToList();

            bool isSceneNoInput = scenes.Any(kvp => kvp.Value == scene.name);

            if (isSceneNoInput)
            {
                PlayerActions.InputActions.Disable();
            }
            else
            {
                PlayerActions.InputActions.Enable();
            }
        }

    }
}