using Scriptable_Object_Templates;
using UnityEngine;

namespace ResourceNodes.Gas
{
    [RequireComponent(typeof(ParticleSystem))]
    public class GasCloudScript : MonoBehaviour, IGatherable
    {
        [SerializeField] private Resource resource;
        public Resource Resource => resource;
    
        [SerializeField] private float gasCapacity = 100f;

        private ParticleSystem particleSystem;

        void Start()
        {
            // Get the Particle System component from the child GameObject
            particleSystem = GetComponentInChildren<ParticleSystem>();
            if (particleSystem == null)
            {
                Debug.LogError("GasCloudController: Particle System not found. Make sure it is a child GameObject.");
            }

            // Ensure that collision is enabled in the Particle System component
            ConfigureParticleCollision();
        }
    

        public void DecreaseGasCapacity(float amount)
        {
            gasCapacity -= amount;

            // TODO: Add animation of gathering later

            // If the gas capacity reaches 0, stop particle emission
            if (gasCapacity <= 0)
            {
                StopGasEmission();
            }
        }

        public float GetGasCapacity()
        {
            return gasCapacity;
        }

        private void StopGasEmission()
        {
            // Stop particle emission when the gas cloud is empty
            if (particleSystem != null)
            {
                particleSystem.Stop();
            }
        }

        private void ConfigureParticleCollision()
        {
            if (particleSystem != null)
            {
                // Enable collision module
                var collisionModule = particleSystem.collision;
                collisionModule.enabled = true;

                // collision settings 
                collisionModule.bounce = 0.5f;
                collisionModule.lifetimeLoss = 0.5f;
           
            }
        }
    }
}