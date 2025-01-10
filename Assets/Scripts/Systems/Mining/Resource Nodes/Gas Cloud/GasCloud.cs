using System.Collections.Generic;
using Systems.Mining.Resource_Nodes.Base;
using UnityEngine;

namespace Systems.Mining.Resource_Nodes.Gas_Cloud
{
    public class GasCloud : ResourceNode
    {
        [SerializeField] private ParticleSystem gasParticleSystem;
        
        private readonly List<ParticleSystem.Particle> _particles = new();
    
        private void OnParticleTrigger()
        {
            var triggeredCount = 
                gasParticleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

            if (triggeredCount == 0)
            {
                return;
            }
            
            for (int i = 0; i < triggeredCount; i++)
            {
                var particle = _particles[i];
                particle.remainingLifetime = 0;
                _particles[i] = particle;
            }
            
            gasParticleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter,_particles);
        }

        protected override void OnLaserInteraction()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnBombInteraction()
        {
            throw new System.NotImplementedException();
        }
    }
}