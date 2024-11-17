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
            Debug.Log("shatter 1");
            
            var position = wholeMesh.transform.position;
            
            shatteredMesh.transform.position = position;
            shatteredMesh.transform.rotation = wholeMesh.transform.localRotation;
            
            var initialVelocity = wholeMesh.GetComponent<Rigidbody>().velocity;
            
            Debug.Log("shatter 2");
            
            Destroy(wholeMesh);
            Debug.Log("shatter 2.1");
            shatteredMesh.SetActive(true);
            Debug.Log("shatter 2.2");
        
            var colliders = Physics.OverlapSphere(position, explosionRadius);
        
            Debug.Log("shatter 2.3");
            
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
            
            Debug.Log("shatter 3");
            
            base.ApplyEffect();
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wholeMesh.transform.position, explosionRadius);
        }
    }
}