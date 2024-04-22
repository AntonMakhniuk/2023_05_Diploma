using UnityEngine;

namespace Scenes.ForTesting.Production_Test
{
    public class TestCanvasToggler : MonoBehaviour
    {
        public GameObject[] testObjects;

        public void ToggleTestObjects()
        {
            foreach (var obj in testObjects)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }
    }
}
