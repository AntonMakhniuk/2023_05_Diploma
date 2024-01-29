using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCloudController : MonoBehaviour
{
    public float initialGasAmount = 100f;
    public float currentGasAmount;

    void Start()
    {
        currentGasAmount = initialGasAmount;
    }
    

    public void CollectGas(float collectedGas)
    {
        currentGasAmount -= collectedGas;
        currentGasAmount = Mathf.Max(0, currentGasAmount);

        // Gas Cloud disappears when gas amount is zero
        if (currentGasAmount <= 0)
        {
            Destroy(gameObject);
        }
    }
    
}
