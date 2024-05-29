using UnityEngine;

namespace Miscellaneous
{
    public class SceneTypeContainer : MonoBehaviour
    {
        public SceneType sceneType;
    }

    public enum SceneType
    {
        MainMenu, Overworld
    }
}