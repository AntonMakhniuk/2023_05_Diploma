using System;
using UnityEngine;

namespace Resource_Nodes.Asteroid
{
    public class AsteroidPoint : MonoBehaviour
    {
        public event EventHandler<AsteroidPoint> OnPointDestroyed;

        // Should be added as the method called by the UnityEvent inside the material associated with the point
        private void ProcessDestruction()
        {
            //A bit of a dumb way to do it, but makes communication really easy
            OnPointDestroyed?.Invoke(this, this);
            
            //TODO: Play some kind of animation?
            
            Destroy(gameObject);
        }
    }
}
