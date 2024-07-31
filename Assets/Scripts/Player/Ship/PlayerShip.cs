using UnityEngine;

namespace Player.Ship
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