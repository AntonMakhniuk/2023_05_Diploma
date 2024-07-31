using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class GasCloudScript : MonoBehaviour
{
    ParticleSystem gasCloud;
    private List<ParticleSystem.Particle> particles;
    
    void OnParticleTrigger()
    {
        
            int triggeredParticles = gasCloud.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
            for (int i = 0; i < triggeredParticles; i++)
            {
                ParticleSystem.Particle p = particles[i];
                p.remainingLifetime = 0;
                particles[i] = p;
            }
            gasCloud.SetTriggerParticles(ParticleSystemTriggerEventType.Enter,particles);
        
    }
    
}