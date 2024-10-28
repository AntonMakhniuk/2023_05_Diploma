using UnityEngine;


public class Crosshair : MonoBehaviour
{ 
    [SerializeField] private RectTransform crosshairUI;
    [SerializeField] private float returnSpeed = 5f;
    [SerializeField] private float smoothing = 0.1f;
    
    private PlayerInputActions _playerInputActions;
    private Vector2 targetPosition;
    private Vector2 smoothPosition;
    private Vector2 screenCenter;

    void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        targetPosition = screenCenter;
        smoothPosition = screenCenter;
    }

    void OnEnable()
    {
        _playerInputActions.PlayerCamera.CameraMovement.Enable();
    }

    void OnDisable()
    {
        _playerInputActions.PlayerCamera.CameraMovement.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 mouseDelta = _playerInputActions.PlayerCamera.CameraMovement.ReadValue<Vector2>();

        if (mouseDelta != Vector2.zero)
        {
            targetPosition += mouseDelta;
        }
        else
        {
            targetPosition = Vector2.Lerp(targetPosition, screenCenter, Time.deltaTime * returnSpeed);
        }
        
        targetPosition.x = Mathf.Clamp(targetPosition.x, 0, Screen.width);
        targetPosition.y = Mathf.Clamp(targetPosition.y, 0, Screen.height);
        
        smoothPosition = Vector2.Lerp(smoothPosition, targetPosition, smoothing);
        
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(crosshairUI.parent as RectTransform, smoothPosition, null, out Vector2 localPoint))
        {
            crosshairUI.localPosition = localPoint;
        }
    }
}
