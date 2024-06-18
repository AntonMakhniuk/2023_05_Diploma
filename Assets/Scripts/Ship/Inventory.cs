using System.Collections.Generic;
using Assets.Scripts.Instruments;
using UnityEngine;

using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Instrument instrument1;
    [SerializeField] private Instrument instrument2;
    [SerializeField] private Instrument instrument3;
    
    [SerializeField] private Transform slot1;
    [SerializeField] private Transform slot2;
    [SerializeField] private Transform slot3;
    
    
    private Instrument activeInstrument;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            instrument1.transform.SetParent(slot1);
            ToggleInstrument(instrument1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            instrument2.transform.SetParent(slot2);
            ToggleInstrument(instrument2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            instrument3.transform.SetParent(slot3);
            ToggleInstrument(instrument3);
        }
    }

    void ToggleInstrument(Instrument instrument)
    {
        if (instrument == activeInstrument)
        {
            activeInstrument.Deactivate();
            activeInstrument = null;
        }
        else
        {
            if (activeInstrument != null)
            {
                activeInstrument.Deactivate();
            }
            instrument.Activate();
            activeInstrument = instrument;
        }
    }
}
