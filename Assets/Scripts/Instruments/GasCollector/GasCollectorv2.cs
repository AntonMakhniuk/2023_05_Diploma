using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCollectorv2 : MonoBehaviour
{
  public float gasCapacityMax = 100f;
    public float gasCollectSpeed = 10f;
    public float gasCapacity = 0f;
    public bool isCollecting = false;
    public bool canCollectGas = false;
    public float GasCollectorOffset { get; set; }
    private Collider currentGasCloudCollider; // Store the Gas Cloud collider
    Rigidbody playerRb;

    // Initialization
    void Start()
    {
        UpdateGasCollectorPosition();
        playerRb = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isCollecting = true;

            // Check if Gas Cloud is in the collector zone when the button is pressed
            if (canCollectGas)
            {
                CollectGas();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            canCollectGas = false;
            isCollecting = false; // Stop collecting when the mouse button is released
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);
        if (other.CompareTag("Gas"))
        {
            // Gas Cloud is within the collector zone
            canCollectGas = true;

            // Store the Gas Cloud collider
            currentGasCloudCollider = other;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Gas"))
        {
            // Gas Cloud exited the collector zone
            canCollectGas = false;

            // Clear the stored Gas Cloud collider
            currentGasCloudCollider = null;
        }
    }

    void CollectGas()
    {
        if (isCollecting && currentGasCloudCollider != null)
        {
            // Move the player ship and collect gas
            float forceMultiplier = gasCollectSpeed * Time.deltaTime;
            playerRb.AddForce(transform.forward * forceMultiplier);



            // Get the GasCloudController script from the Gas Cloud object
            GasCloudController gasCloudController = currentGasCloudCollider.GetComponent<GasCloudController>();

            if (gasCloudController != null)
            {
                // Collect gas from the Gas Cloud
                float collectedGas = gasCollectSpeed * Time.deltaTime;
                gasCloudController.CollectGas(collectedGas);

                // Reduce gas from the GasCollector and update UI or other game elements
                gasCapacity += collectedGas;
                gasCapacity = Mathf.Min(gasCapacity, gasCapacityMax);
            }
        }
    }

    public float GetCurrentGasStorage()
    {
        return gasCapacity;
    }

    void UpdateGasCollectorPosition()
    {
        transform.position = transform.parent.position - transform.parent.up * GasCollectorOffset;
    }
}
