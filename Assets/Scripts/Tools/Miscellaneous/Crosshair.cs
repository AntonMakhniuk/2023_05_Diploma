using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{ 
    [SerializeField] private RectTransform crosshairUI;
    private PlayerInputActions _playerInputActions;

    void Awake()
    {
        _playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        _playerInputActions.PlayerCamera.CameraMovement.Enable();
    }

    void OnDisable()
    {
        _playerInputActions.PlayerCamera.CameraMovement.Disable();
    }

    private void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(crosshairUI.parent as RectTransform, mousePosition, null, out Vector2 localPoint))
        {
            crosshairUI.localPosition = localPoint;
        }
    }
}
