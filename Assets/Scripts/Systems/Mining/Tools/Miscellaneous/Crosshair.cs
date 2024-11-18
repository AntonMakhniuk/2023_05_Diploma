using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Mining.Tools.Miscellaneous
{
    public class Crosshair : MonoBehaviour
    { 
        [SerializeField] private RectTransform crosshairUI;
        [SerializeField] public float maxRadius = 100f;
        private PlayerInputActions _playerInputActions;

        void Awake()
        {
            _playerInputActions = PlayerActions.InputActions;
        }

        void OnEnable()
        {
            _playerInputActions.PlayerCamera.CameraMovement.Enable();
        }

        void OnDisable()
        {
            _playerInputActions.PlayerCamera.CameraMovement.Disable();
        }

        void Update()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
        
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(crosshairUI.parent as RectTransform, mousePosition, null, out Vector2 localPoint))
            {
                if (localPoint.magnitude > maxRadius)
                {
                    localPoint = localPoint.normalized * maxRadius;
                }
                crosshairUI.localPosition = localPoint;
            }
        }
    }
}