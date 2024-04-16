using UnityEngine;

namespace Player
{
    public class PlayerActions : MonoBehaviour
    {
        public static PlayerInputActions InputActions { get; private set; }

        private void Awake()
        {
            InputActions = new PlayerInputActions();
            
            InputActions.Enable(); 
        }
    }
}