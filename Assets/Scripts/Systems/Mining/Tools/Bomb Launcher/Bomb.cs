using System;
using System.Collections;
using Systems.Mining.Resource_Nodes;
using Systems.Mining.Resource_Nodes.Base;
using Systems.Mining.Tools.Base_Tools;
using Tools.Base_Tools;
using UnityEngine;

namespace Systems.Mining.Tools.Bomb_Launcher
{
    public class Bomb : MonoBehaviour
    {
        private const float FlightTime = 3f;
        private const float ExplosionRadius = 5f;
        private const float MaxExplosionDamage = 50f;
        
        [SerializeField] private GameObject explosion;

        private void Start()
        {
            StartCoroutine(ApplyVelocity());
        }

        private IEnumerator ApplyVelocity()
        {
            var rb = GetComponent<Rigidbody>();
            var initialSpeed = rb.velocity.magnitude;
            var currentTime = 0f;

            while (currentTime < FlightTime)
            {
                var currentSpeed = Mathf.Lerp(initialSpeed, 0, currentTime / FlightTime);
                
                rb.velocity = transform.forward * currentSpeed;
                currentTime += Time.deltaTime;
                
                yield return null;
            }
            
            rb.velocity = Vector3.zero;
        }
    
        public void Detonate()
        {
            var colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
            
            foreach (var overlappingCollider in colliders)
            {
                if (overlappingCollider.TryGetComponent<ResourceNodeWithHealth>(out var nodeWithHealth))
                {
                    nodeWithHealth.Interact(ToolType.Bomb, Vector3.Distance(transform.position, 
                        overlappingCollider.transform.position) / ExplosionRadius * MaxExplosionDamage);
                }
                else if (overlappingCollider.TryGetComponent<ResourceNode>(out var node))
                {
                    node.Interact(ToolType.Bomb);
                }
            }

            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public event EventHandler<Bomb> OnBombDestroyed;

        private void OnDestroy()
        {
            OnBombDestroyed?.Invoke(this, this);
        }
    }
}