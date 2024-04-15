using UnityEngine;
using System;
using System.Collections.Generic;

public class Teleporter : BuildingObject
{
    protected override void Awake()
    {
        base.Awake(); 
    }

    protected override void OnDestroy()
    {
        base.OnDestroy(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        TeleporterUI teleporterUI = TeleporterUI.GetInstance();
        if (other.CompareTag("Player") && teleporterUI != null)
        {
            List<Teleporter> teleporters = GetInstancesOfType<Teleporter>();

            teleporters.Remove(this);

            teleporterUI.ShowTeleporterUI(teleporters, this);
        }
    }
}
