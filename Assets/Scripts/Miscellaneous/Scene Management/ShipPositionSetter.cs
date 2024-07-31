using Player;
using Player.Ship;
using UnityEngine;

namespace Miscellaneous
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