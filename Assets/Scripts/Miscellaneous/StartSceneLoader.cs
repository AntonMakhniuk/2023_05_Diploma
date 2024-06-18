using UnityEngine;

namespace Miscellaneous
{
    public class StartSceneLoader: MonoBehaviour
    {
        [SerializeField] private SceneTypeContainer sceneTypeContainer;
        
        private void Start()
        {
            LevelManager.ChangeScene(sceneTypeContainer);
        }
    }
}