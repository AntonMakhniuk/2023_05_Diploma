using Systems.Mining.Addons;
using UnityEngine;

namespace Systems.Mining.Transitions.Transition_Addons
{
    public class ShatterAddon : BaseTransitionAddon
    {
        [Header("Mesh Data")]
        [SerializeField] private GameObject wholeMesh;
        [SerializeField] private GameObject shatteredMesh;
        [Space] 
        [Header("Shatter Data")] 
        [SerializeField] private float explosionRadius = 1f;
        [SerializeField] private float explosionPower = 1f;

        public override void ApplyEffect()
        {
            var position = wholeMesh.transform.position;
            
            shatteredMesh.transform.position = position;
            shatteredMesh.transform.rotation = wholeMesh.transform.rotation;
            
            var initialVelocity = wholeMesh.GetComponent<Rigidbody>().velocity;
            
            Destroy(wholeMesh);
            shatteredMesh.SetActive(true);
        
            var colliders = Physics.OverlapSphere(position, explosionRadius);
            
            foreach (var overlappingCollider in colliders)
            {
                var rb = overlappingCollider.GetComponent<Rigidbody>();
        
                if (rb == null)
                {
                    continue;   
                }

                rb.velocity = initialVelocity;
                rb.AddExplosionForce(explosionPower, position, explosionRadius, 0);
            }
            
            base.ApplyEffect();
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wholeMesh.transform.position, explosionRadius);
        }
    }
}