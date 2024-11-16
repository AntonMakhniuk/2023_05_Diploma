using UnityEngine;

[ExecuteAlways]
public class MapBoundaryController : MonoBehaviour
{
    [Header("Drone Setup")]
    [Tooltip("The drone Script to monitor boundaries.")]
    [SerializeField] private DroneController droneController;
    [Tooltip("The drone GameObject to monitor boundaries.")]
    [SerializeField] private Transform droneBody;

    [Header("Zone Radius")]
    [Tooltip("Radius of the free movement zone.")]
    [Range(10f, 1500f)]
    [SerializeField] private float zone1Radius = 50f;

    [Tooltip("Radius of the deceleration zone.")]
    [Range(10f, 3000f)]
    [SerializeField] private float zone2Radius = 100f;

    [Tooltip("Radius of the forced alignment zone.")]
    [Range(10f, 4500f)]
    [SerializeField] private float zone3Radius = 150f;

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
        _droneRigidbody = droneController.GetComponent<Rigidbody>();
        droneBody = droneController.transform;
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(droneBody.position, transform.position);
        Vector3 directionToStation = (transform.position - droneBody.position).normalized;

        if (distance < zone1Radius)
        {
            droneController.SetThrust(1500f); 
        }
        else if (distance < zone2Radius)
        {
            float decelerationFactor = Mathf.Clamp01((distance - zone1Radius) / (zone2Radius - zone1Radius));
            
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
            if (distance >= zone3Radius)
            {
                droneBody.position = transform.position + directionToStation * zone2Radius;
            }
            else
            {
                _droneRigidbody.velocity = Vector3.Lerp(_droneRigidbody.velocity, directionToStation * forcedReturnSpeed, Time.fixedDeltaTime);
                droneBody.forward = Vector3.Lerp(droneBody.forward, directionToStation, Time.fixedDeltaTime);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            zone2Radius = Mathf.Max(zone2Radius, zone1Radius);
            zone3Radius = Mathf.Max(zone3Radius, zone2Radius);
        }

        // Zone 1: Free movement
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, zone1Radius);

        // Zone 2: Deceleration
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, zone2Radius);

        // Zone 3: Forced alignment
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, zone3Radius);
    }
}
