using System;
using Systems.Mining.Tools.Base_Tools;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO: Should be rewritten
namespace Player.Ship
{
    public class ToolInventory : MonoBehaviour
    {
        [SerializeField] private BaseTool instrument1;
        [SerializeField] private BaseTool instrument2;
        [SerializeField] private BaseTool instrument3;
        [SerializeField] private BaseTool marker;
        
        private BaseTool _activeBaseTool;
        
        private Action<InputAction.CallbackContext> _toggleInstrument1;
        private Action<InputAction.CallbackContext> _toggleInstrument2;
        private Action<InputAction.CallbackContext> _toggleInstrument3;
        private Action<InputAction.CallbackContext> _toggleMarker;

        private void Start()
        {
            _toggleInstrument1 = _ => ToggleInstrument(instrument1);
            _toggleInstrument2 = _ => ToggleInstrument(instrument2);
            _toggleInstrument3 = _ => ToggleInstrument(instrument3);
            _toggleMarker = _ => ToggleInstrument(marker);

            PlayerActions.InputActions.PlayerShip.ToggleSlot_1.performed += _toggleInstrument1;
            PlayerActions.InputActions.PlayerShip.ToggleSlot_2.performed += _toggleInstrument2;
            PlayerActions.InputActions.PlayerShip.ToggleSlot_3.performed += _toggleInstrument3;
            PlayerActions.InputActions.PlayerShip.ToggleMarker.performed += _toggleMarker;
        }

        private void ToggleInstrument(BaseTool tool)
        {
            if (_activeBaseTool != null)
            {
                _activeBaseTool.IsActiveTool = false;
            }
            
            if (tool == _activeBaseTool)
            {
                _activeBaseTool = null;
            }
            else
            {
                tool.IsActiveTool = true;
                
                _activeBaseTool = tool;
            }
        }

        private void OnDestroy()
        {
            PlayerActions.InputActions.PlayerShip.ToggleSlot_1.performed -= _toggleInstrument1;
            PlayerActions.InputActions.PlayerShip.ToggleSlot_2.performed -= _toggleInstrument2;
            PlayerActions.InputActions.PlayerShip.ToggleSlot_3.performed -= _toggleInstrument3;
            PlayerActions.InputActions.PlayerShip.ToggleMarker.performed -= _toggleMarker;
        }
    }
}
