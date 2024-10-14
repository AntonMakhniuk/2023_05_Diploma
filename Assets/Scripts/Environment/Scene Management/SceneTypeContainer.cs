using UnityEngine;

namespace Miscellaneous.Scene_Management
{
    public class SceneTypeContainer : MonoBehaviour
    {
        public SceneType sceneType;
    }

    public enum SceneType
    {
        MainMenu, Overworld, AsteroidField
    }
}   