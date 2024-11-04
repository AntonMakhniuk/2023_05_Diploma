using UnityEngine;

namespace Player.Ship
{
    public class PlayerShipMap : MonoBehaviour
    {
        public static PlayerShipMap Instance;

        private void Awake()
        {
            Instance = this;
        }
    }
}