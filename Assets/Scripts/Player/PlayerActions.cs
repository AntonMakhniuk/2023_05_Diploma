using UnityEngine;

namespace Player
{
    public class PlayerActions : MonoBehaviour
    {
        private static PlayerInputActions _inputActions;
        public static PlayerInputActions InputActions
        {
            get
            {
                if (_inputActions != null)
                {
                    return _inputActions;
                }

                _inputActions = new PlayerInputActions();
                
                _inputActions.Enable();

                return _inputActions;
            }
        }

        private void Awake()
        {
            if (_inputActions != null)
            {
                return;
            }
            
            _inputActions = new PlayerInputActions();
            
            _inputActions.Enable(); 
        }
    }
}