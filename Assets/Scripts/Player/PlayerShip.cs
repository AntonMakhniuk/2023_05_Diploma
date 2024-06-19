using UnityEngine;

namespace Player
{
    public class PlayerShip : MonoBehaviour
    {
        public static PlayerShip Instance;

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
    }
}