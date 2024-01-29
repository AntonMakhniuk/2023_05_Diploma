using System;
using Assets.Scripts.Instruments;
using UnityEngine;

public class ShipInventory : MonoBehaviour {
    [SerializeField] private TractorBeam tractorBeam;
    [SerializeField] private Drill drill;
    [SerializeField] private GasCollectorV3 gasCollector;

    [SerializeField] private Instrument activeInstrument = null;
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.PlayerShip.ToggleTractorBeam.performed += context => { ToggleInstrument(tractorBeam); };
        playerInputActions.PlayerShip.ToggleDrill.performed += context => { ToggleInstrument(drill); };
        playerInputActions.PlayerShip.ToggleGasCollector.performed += context => { ToggleInstrument(gasCollector); };
    }

    private void ToggleInstrument(Instrument instr) {
        if (activeInstrument == null) {
            instr.Toggle();
            activeInstrument = instr;
        }
        else if (activeInstrument == instr) {
            instr.Toggle();
            activeInstrument = null;
        }
    }
}