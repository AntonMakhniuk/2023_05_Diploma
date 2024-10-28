using Player.Ship;
using UnityEngine;

namespace Environment.Scene_Management
{
    public class ShipPositionSetter : MonoBehaviour
    {
        private void Awake()
        {
            PlayerShip.Instance.transform.position = gameObject.transform.position;
            // ReSharper disable twice Unity.InefficientPropertyAccess
            PlayerShip.Instance.transform.rotation = gameObject.transform.rotation;
        }
    }
}