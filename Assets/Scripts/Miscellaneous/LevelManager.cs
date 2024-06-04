using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Miscellaneous
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
        
        [SerializedDictionary("Scene Type", "Associated Scene Name")]
        public SerializedDictionary<SceneType, string> scenesDict;
        
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
            SceneManager.LoadSceneAsync(Instance.scenesDict[sceneTypeContainer.sceneType]);
        }
        
        public static void Quit()
        {
            Application.Quit();
        }
    }
}