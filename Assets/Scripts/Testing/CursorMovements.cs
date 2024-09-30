using Player;
using UnityEngine;

namespace Testing
{
    public class CursorMovements : MonoBehaviour
    {
        [SerializeField] private Transform ship;
        [SerializeField] private float distanceFromShip = 15f;
        [SerializeField] private float followSmoothing = 0.1f;

        private Vector3 currentVelocity = Vector3.zero;
        private PlayerInputActions _playerInputActions;
        
        private void Awake()
        {

            _playerInputActions = PlayerActions.InputActions;
            _playerInputActions.PlayerCamera.Enable();
        }

        void FixedUpdate()
        {
            if (_playerInputActions.PlayerCamera.MousePosition.WasPerformedThisFrame())
            {
                Vector2 mousePosition = _playerInputActions.PlayerCamera.MousePosition.ReadValue<Vector2>();
            
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);

                Vector3 targetPosition = ray.GetPoint(distanceFromShip);


                Vector3 smoothPosition =
                    Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, followSmoothing);
                transform.position = smoothPosition;
            }
            
        }
    }
}
