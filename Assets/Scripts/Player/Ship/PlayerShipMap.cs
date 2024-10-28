using UnityEngine;

namespace Player.Ship
{
    public class PlayerShipMap : MonoBehaviour
    {
        public static PlayerShipMap Instance;

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