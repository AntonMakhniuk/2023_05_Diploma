using Assets.Scripts.Instruments;
using UnityEngine;

public class GasCollector : Instrument
{
    [SerializeField]private string gasCloudTag = "Gas"; 
    [SerializeField]private float gatheringSpeed = 5f;
    [SerializeField]private float maxGasStorage = 100f;
    public float GasCollectorOffset { get; set; } 
    
    [SerializeField]private float currentGasStorage = 0f;
    private bool isActivated = false;

    void Start()
    {
        GasCollectorOffset = 0; // Set the default offset
    }

    void Update()
    {
        // Activate Gas Collector
        if (isActivated)
        {
            UpdateGasCollectorPosition();
            RotateGasCollector();
        }

        // Gather gas when activated and left mouse button is pressed
        if (Input.GetMouseButton(0) && isActivated)
        {
            GatherGas();
        }
    }

    void RotateGasCollector()
    {
        transform.rotation = transform.parent.rotation;
    }

    void UpdateGasCollectorPosition()
    {
        transform.position = transform.parent.position - transform.parent.up * GasCollectorOffset;
    }

    void GatherGas()
    {
        // Find gas clouds with the specified tag
        GameObject[] gasClouds = GameObject.FindGameObjectsWithTag(gasCloudTag);

        foreach (GameObject gasCloud in gasClouds)
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
                UpdateGasCloudCapacity(gasCloud, gasToGather);
            }
        }
    }

    void UpdateGasCloudCapacity(GameObject gasCloud, float gasGathered)
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

    public void ActivateGasCollector()
    {
        isActivated = true;
    }

    public void DeactivateGasCollector()
    {
        isActivated = false;
    }

    public float GetCurrentGasStorage()
    {
        return currentGasStorage;
    }
}
