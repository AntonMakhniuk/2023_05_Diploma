using UnityEngine;
using System;
using System.Collections.Generic;

public class Teleporter : BuildingObject
{

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
