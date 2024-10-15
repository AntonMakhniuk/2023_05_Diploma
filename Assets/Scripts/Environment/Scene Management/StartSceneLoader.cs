using UnityEngine;

namespace Environment.Scene_Management
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