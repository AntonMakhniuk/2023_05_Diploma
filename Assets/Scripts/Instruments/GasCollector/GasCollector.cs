using System;
using ResourceNodes.Gas;
using UnityEngine;

public class GasCollector : MonoBehaviour
{
    public KeyCode gatherKey = KeyCode.Mouse0;

    [SerializeField] private float gatheringSpeed = 5f;
    [SerializeField] private float maxGasStorage = 100f;
    public float GasCollectorOffset { get; set; }

    [SerializeField] private float currentGasStorage = 0f;
    private bool isGathering = false;

    void Start()
    {
        GasCollectorOffset = 0;
    }

    void Update()
    {
        UpdateGasCollectorPosition();

        // Toggle gathering with the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            isGathering = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isGathering = false;
        }

        
    }

    void OnParticleCollision(GameObject other)
    {
        // Check if the collided object is a Gas Cloud particle
        if (other.CompareTag("Gas") && isGathering)
        {
            // Handle the collision, gather gas, etc.
            GasCloudScript gasCloudController = other.GetComponentInParent<GasCloudScript>();
            if (gasCloudController != null)
            {
                float gasGathered = gatheringSpeed * Time.deltaTime;
                gasCloudController.DecreaseGasCapacity(gasGathered);

                // Update the Gas Collector's gas storage
                currentGasStorage += gasGathered;
            }
        }
    }

    private void OnParticleTrigger() {
        
    }

    void UpdateGasCollectorPosition()
    {
        transform.position = transform.parent.position - transform.parent.up * GasCollectorOffset;

    }

    public float GetCurrentGasStorage()
    {
        return currentGasStorage;
    }
}