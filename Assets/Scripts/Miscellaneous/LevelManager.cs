using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Miscellaneous
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
        
        [SerializedDictionary("Scene Type", "Associated Scene Names")]
        public SerializedDictionary<SceneType, List<string>> scenesDict;
        
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

        public static void ChangeScene(SceneTypeContainer sceneTypeContainer)
        {
            var scenesToLoad = Instance.scenesDict[sceneTypeContainer.sceneType];

            SceneManager.LoadScene(scenesToLoad[0]);

            if (scenesToLoad.Count <= 1)
            {
                return;
            }
            
            for (int i = 1; i < scenesToLoad.Count; i++)
            {
                SceneManager.LoadScene(scenesToLoad[i], LoadSceneMode.Additive);
            }
        }
        
        public static void Quit()
        {
            Application.Quit();
        }
    }
}