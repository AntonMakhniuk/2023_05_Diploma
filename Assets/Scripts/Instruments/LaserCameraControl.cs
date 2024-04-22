using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LaserCameraControl : MonoBehaviour {
    private PlayerInputActions _playerInputActions;
    private Rigidbody _rb;
    private Transform _mainCamera;
    
    [SerializeField] private Transform laserBarrel;
    [SerializeField] private Transform laserLeg;
    [SerializeField] private float rotationSpeed;
    
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _mainCamera = Camera.main.transform;
        
        _playerInputActions.PlayerCamera.Enable();
    }

    private void Update() {
        RotateWithCamera();
    }

    public void RotateWithCamera() 
    {
        Quaternion targetRotationLeg = Quaternion.Euler(0, _mainCamera.eulerAngles.y, 0);
        laserLeg.rotation = Quaternion.Lerp(laserLeg.rotation, targetRotationLeg, rotationSpeed * Time.deltaTime);
        
        Quaternion targetRotationBarrel = Quaternion.Euler(_mainCamera.eulerAngles.x, laserLeg.eulerAngles.y, 0);
        laserBarrel.rotation = Quaternion.Lerp(laserBarrel.rotation, targetRotationBarrel, rotationSpeed * Time.deltaTime);
    }
}
