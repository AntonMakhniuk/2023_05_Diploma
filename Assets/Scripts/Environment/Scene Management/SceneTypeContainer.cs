using UnityEngine;

namespace Environment.Scene_Management
{
    public class SceneTypeContainer : MonoBehaviour
    {
        public SceneType sceneType;
    }

    public enum SceneType
    {
        MainMenu, Overworld, AsteroidField, GlobalMap, AsteroidFieldWithEnemies
    }
}   