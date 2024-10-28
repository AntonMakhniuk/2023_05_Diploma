using Building.Buildings.Base_Classes;
using Player.Ship;
using UnityEngine;

namespace Building.Structures
{
    public class Teleporter : BaseBuilding
    {
        [SerializeField] private Transform teleportPositionTransform;
        
        public void TeleportToDestination(Teleporter destination)
        {
            PlayerShip.Instance.transform.position = destination.teleportPositionTransform.position;
        }
    }
}
