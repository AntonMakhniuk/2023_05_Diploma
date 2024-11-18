using Systems.Mining.Resource_Nodes.Base;

namespace Systems.Mining.Resource_Nodes.Asteroid
{
    public class Asteroid : ResourceNodeWithHealth
    {
        protected override void OnLaserInteraction()
        {
            // No special interaction
        }

        protected override void OnBombInteraction()
        {
            // No special interaction
        }
    }
}