using UnityEngine;

public class GasCollector : MonoBehaviour
{
    public KeyCode gatherKey = KeyCode.Mouse0;

    [SerializeField]private GameObject gasCloud;
    [SerializeField]private float gatheringSpeed = 5f;
    [SerializeField]private float maxGasStorage = 100f;
    public float GasCollectorOffset { get; set; }

    private float currentGasStorage = 0f;
    private Rigidbody shipRigidbody; // Reference to the ship's rigidbody

    void Start()
    {
        GasCollectorOffset = 0; 

        // Get the ship's rigidbody
        shipRigidbody = GetComponentInParent<Rigidbody>();
        if (shipRigidbody == null)
        {
            Debug.LogError("GasCollector requires a Rigidbody on the parent GameObject (ship).");
        }
    }

    void Update()
    {
        UpdateGasCollectorPosition();
        // Gather gas when the left mouse button is pressed, the gas collector is activated, and the ship is moving
        if (Input.GetMouseButton(0) && IsShipMoving())
        {
            GatherGas();
        }
    }
    void OnParticleCollision(GameObject other)
    {
        // Check if the collided object is a Gas Cloud particle
        if (other.CompareTag("GasCloudParticle"))
        {
            // Handle the collision, gather gas, etc.
            GasCloudScript gasCloudController = other.GetComponentInParent<GasCloudScript>();
            if (gasCloudController != null)
            {
                float gasGathered = 10f;
                gasCloudController.DecreaseGasCapacity(gasGathered);
            }
        }
    }

    void GatherGas()
    {
        if (gasCloud != null)
        {
            // Move Gas Collector towards the Gas Cloud
            transform.position = Vector3.MoveTowards(transform.position, gasCloud.transform.position, (gatheringSpeed * Time.deltaTime) + GasCollectorOffset);

            // Calculate the amount of gas to gather based on the gathering speed
            float gasToGather = gatheringSpeed * Time.deltaTime;

            // Check if there is enough space in the gas storage
            if (currentGasStorage + gasToGather <= maxGasStorage)
            {
                // Gather gas and update the storage
                currentGasStorage += gasToGather;

                // Decrease gas capacity in the gas cloud
                UpdateGasCloudCapacity(gasToGather);
            }
        }
    }
    void UpdateGasCollectorPosition()
    {
        transform.position = transform.parent.position - transform.parent.up * GasCollectorOffset;
    }
    void UpdateGasCloudCapacity(float gasGathered)
    {
        GasCloudScript gasCloudScript = gasCloud.GetComponent<GasCloudScript>();

        if (gasCloudScript != null)
        {
            gasCloudScript.DecreaseGasCapacity(gasGathered);

            // If the gas capacity reaches 0, make the cloud disappear
            if (gasCloudScript.GetGasCapacity() <= 0)
            {
                Destroy(gasCloud);
            }
        }
    }

    bool IsShipMoving()
    {
        // Check if the ship's velocity is above a certain threshold
        return shipRigidbody.velocity.magnitude > 0.1f;
    }

    public float GetCurrentGasStorage()
    {
        return currentGasStorage;
    }
}
