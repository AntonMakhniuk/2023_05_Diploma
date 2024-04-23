using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Instruments;
using UnityEngine;

public class GasCollectorV4 : Instrument
{
     
    [SerializeField]private int maxGasCapacity = 100; 

    [SerializeField]private bool isCollectorActive = false;
    [SerializeField]private bool isGatheringGas = false;
    [SerializeField]private int currentGasLevel = 0;
    private InventoryWindow inventory;
    
    protected override void Awake() {
        base.Awake();
        inventory = GetComponentInParent<InventoryWindow>();
    }
    
    void Update()
    {
        
        if (isCollectorActive && Input.GetMouseButtonDown(0))
        {
            isGatheringGas = true;
        }
        else if (isCollectorActive && Input.GetMouseButtonUp(0))
        {
            isGatheringGas = false;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (isCollectorActive && isGatheringGas)
        {
            if (other.CompareTag("Gas"))
            {
                if (currentGasLevel < maxGasCapacity)
                {
                    inventory.IncreaseFuelGasQuantity(1f);
                    currentGasLevel++;
                }
                else
                {
                    isGatheringGas = false;
                }
            }
        }
    }
    
    public override void Toggle()
    {
        isCollectorActive = !isCollectorActive;
        base.Toggle();
        if (!isCollectorActive)
        {
            isGatheringGas = false;
        }
    }
}
