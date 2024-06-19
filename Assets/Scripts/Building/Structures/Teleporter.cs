using Building.Systems;
using Player;

namespace Building.Structures
{
    public class Teleporter : BuildingObject
    {
        public void TeleportToDestination(Teleporter destination)
        {
            var teleportPosition = destination.transform.position + destination.transform.forward * 3f;
            
            PlayerShip.Instance.transform.position = teleportPosition;
        }
    }
}
