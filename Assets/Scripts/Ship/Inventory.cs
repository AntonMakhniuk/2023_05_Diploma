using System.Collections.Generic;
using Assets.Scripts.Instruments;
using UnityEngine;

using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private LaserV3 laserController;
    [SerializeField] private TractorBeamController tractorBeamController;
    [SerializeField] private BombContainer bombContainer;
    
    private Instrument activeInstrument;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleInstrument(laserController);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToggleInstrument(tractorBeamController);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ToggleInstrument(bombContainer);
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
