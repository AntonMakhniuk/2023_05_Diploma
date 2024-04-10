using System;
using Assets.Scripts.Instruments;
using UnityEngine;

public class ShipInventory : MonoBehaviour {
    [SerializeField] private TractorBeam tractorBeam;
    [SerializeField] private Drill drill;

    [SerializeField] private BombContainer bombContainer;

    [SerializeField] private GasCollectorV4 gasCollector;
    [SerializeField] private Laser laser;

    [SerializeField] private Instrument activeInstrument = null;
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.PlayerShip.ToggleTractorBeam.performed += context => { ToggleInstrument(tractorBeam); };
        playerInputActions.PlayerShip.ToggleDrill.performed += context => { ToggleInstrument(drill); };
        playerInputActions.PlayerShip.ToggleGasCollector.performed += context => { ToggleInstrument(gasCollector); };

        playerInputActions.PlayerShip.ToggleBombContainer.performed += context => { ToggleInstrument(bombContainer); };
        playerInputActions.PlayerShip.ToggleLaser.performed += context => { ToggleInstrument(laser); };
        
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