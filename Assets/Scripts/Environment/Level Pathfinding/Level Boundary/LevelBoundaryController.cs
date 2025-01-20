using NaughtyAttributes;
using Player.Movement.Drone_Movement;
using Scriptable_Object_Templates.Singletons;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO:Delete when ExecuteAlways when creating build 
namespace Environment.Level_Pathfinding.Level_Boundary
{
    [ExecuteAlways]
    public class LevelBoundaryController : MonoBehaviour
    {
        public static LevelBoundaryController Instance;

        [Expandable]
        public LevelBoundaryData dataObject;
        
        [Header("Drone Setup")]
        [Tooltip("The drone Script to monitor boundaries.")]
        [SerializeField] private DroneController droneController;
        [Tooltip("The drone GameObject to monitor boundaries.")]
        [SerializeField] private Transform droneBody;

        [Header("Zone 2 Settings")]
        [Tooltip("Factor to scale thrust in the deceleration zone.")]
        [Range(0.1f, 1f)]
        [SerializeField] private float decelerationMultiplier = 0.5f;
    
        [Tooltip("Threshold for alignment between the drone's forward direction and the station direction.")]
        [Range(0.1f, 1f)]
        [SerializeField]
        private float alignmentValue = 0.9f;

        [Header("Zone 3 Alignment Settings")]
        [Tooltip("Speed factor for forced alignment and return in Zone 3.")]
        [Range(1f, 20f)]
        [SerializeField] private float forcedReturnSpeed = 10f;
        
        private Rigidbody _droneRigidbody;
        
        private void Awake()
        {
            Instance = this;
            
            Debug.Log(Instance);
            
            UpdateLevelData(SceneManager.GetActiveScene().name);
            
            _droneRigidbody = droneController.GetComponent<Rigidbody>();
            droneBody = droneController.transform;
        }

        public void UpdateLevelData(string sceneName)
        {
            dataObject = LevelBoundaryDictionary.Instance.dictionary[sceneName];
        }

        private void FixedUpdate()
        {
            if (droneBody == null) { return; }
            float distance = Vector3.Distance(droneBody.position, transform.position);
            Vector3 directionToStation = (transform.position - droneBody.position).normalized;

            if (distance < dataObject.zone1Radius)
            {
                droneController.SetThrust(1500f); 
            }
            else if (distance < dataObject.zone2Radius)
            {
                float decelerationFactor = Mathf.Clamp01((distance - dataObject.zone1Radius) /
                                                         (dataObject.zone2Radius - dataObject.zone1Radius));
            
                float alignment = Vector3.Dot(droneBody.forward, directionToStation);
                if (alignment > alignmentValue)
                {
                    droneController.SetThrust(1500f); 
                }
                else
                {
                    droneController.SetThrust(1500f * (1f - (decelerationMultiplier * decelerationFactor))); 
                }
            }
            else
            {
                if (distance >= dataObject.zone3Radius)
                {
                    droneBody.position = transform.position + directionToStation * dataObject.zone2Radius;
                }
                else
                {
                    _droneRigidbody.velocity = Vector3.Lerp(_droneRigidbody.velocity, 
                        directionToStation * forcedReturnSpeed, Time.fixedDeltaTime);
                    droneBody.forward = Vector3.Lerp(droneBody.forward, directionToStation, Time.fixedDeltaTime);
                }
            }
        }

        public Bounds GetWorldBounds()
        {
            var radius = LevelBoundaryDictionary.Instance
                .dictionary[SceneManager.GetActiveScene().name]
                .zone3Radius;
            
            Debug.Log(transform.position);
            
            return new Bounds
            {
                center = transform.position,
                extents = new Vector3(radius, radius, radius)
            };
        }
        
        private void OnDrawGizmos()
        {
            if (dataObject == null)
            {
                UpdateLevelData(SceneManager.GetActiveScene().name);
            }
            
            // Zone 1: Free movement
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, dataObject.zone1Radius);

            // Zone 2: Deceleration
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, dataObject.zone2Radius);

            // Zone 3: Forced alignment
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, dataObject.zone3Radius);
        }
    }
}
