using Building.Systems;
using Player.Ship;
using UnityEngine;

namespace Building.Structures
{
    public class Teleporter : BuildingObject
    {
        [SerializeField] private Transform teleportPositionTransform;
        
        public void TeleportToDestination(Teleporter destination)
        {
            PlayerShip.Instance.transform.position = destination.teleportPositionTransform.position;
        }
    }
}
