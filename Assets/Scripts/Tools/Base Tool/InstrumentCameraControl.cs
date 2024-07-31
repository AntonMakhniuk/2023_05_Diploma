using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InstrumentCameraControl : MonoBehaviour {
    private PlayerInputActions _playerInputActions;
    private Rigidbody _rb;
    private Transform _mainCamera;
    
    [SerializeField] private Transform barrel;
    [SerializeField] private Transform leg;
    [SerializeField] private float rotationSpeed;
    
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _mainCamera = Camera.main.transform;
        
        //_playerInputActions.PlayerCamera.Enable();
    }

    private void Update() {
        RotateWithCamera();
    }

    public void RotateWithCamera() 
    {
        Quaternion targetRotationLeg = Quaternion.Euler(0, _mainCamera.eulerAngles.y, 0);
        leg.rotation = Quaternion.Lerp(leg.rotation, targetRotationLeg, rotationSpeed * Time.deltaTime);
        
        Quaternion targetRotationBarrel = Quaternion.Euler(_mainCamera.eulerAngles.x, leg.eulerAngles.y, 0);
        barrel.rotation = Quaternion.Lerp(barrel.rotation, targetRotationBarrel, rotationSpeed * Time.deltaTime);
    }
}
