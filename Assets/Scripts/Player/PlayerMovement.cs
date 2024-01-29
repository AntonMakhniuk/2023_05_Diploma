using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {
    private PlayerInputActions playerInputActions;
    private Rigidbody rb;
    private Camera mainCamera;

    // Required for detecting rotation (no need for that for movement since only rotation is read as 3D Vector)
    private bool isRotating = false;

    [Header("Ship Movement Parameters")]
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float accelerationDrag = 0.3f;
    [SerializeField] private float decelerationDrag = 1.5f;
    [Space]
    [Header("Ship Rotation Parameters")]
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float maxAngularVelocity = 1;
    [SerializeField] private float accelerationAngularDrag = 0.1f;
    [SerializeField] private float decelerationAngularDrag = 1f;
    [SerializeField] private float cameraAlignRotationSpeed = 1f;
    
    private void Awake() {
        playerInputActions = new PlayerInputActions();
        
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;
        
        mainCamera = Camera.main;
        
        // Enable the action map
        playerInputActions.Enable(); 
        
        // If the player presses the movement key, the drag will be changed to allow for higher velocity to be reached
        playerInputActions.PlayerShip.Movement.performed += context => { rb.drag = accelerationDrag; };
        // If the player stops pressing the movement key, the drag will be changed to rapidly decelerate the ship
        playerInputActions.PlayerShip.Movement.canceled += context => { rb.drag = decelerationDrag; };

        // If the player presses the rotation key, the drag will be changed to allow for quicker turn speed
        // If the player stops pressing the rotation key, the drag will be changed to rapidly decelerate the ship's rotation
        // Thi is repeated for every axis
        
        // X axis
        playerInputActions.PlayerShip.RotateAlongX.performed += context =>
        {
            rb.angularDrag = accelerationAngularDrag;
            isRotating = true;
        };
        
        playerInputActions.PlayerShip.RotateAlongX.canceled += context =>
        {
            rb.angularDrag = decelerationAngularDrag;
            isRotating = false;
        };
        
        // Y axis
        playerInputActions.PlayerShip.RotateAlongY.performed += context =>
        {
            rb.angularDrag = accelerationAngularDrag;
            isRotating = true;
        };
        
        playerInputActions.PlayerShip.RotateAlongY.canceled += context =>
        {
            rb.angularDrag = decelerationAngularDrag;
            isRotating = false;
        };
        
        // Z axis
        playerInputActions.PlayerShip.RotateAlongZ.performed += context =>
        {
            rb.angularDrag = accelerationAngularDrag;
            isRotating = true;
        };
        
        playerInputActions.PlayerShip.RotateAlongZ.canceled += context =>
        {
            rb.angularDrag = decelerationAngularDrag;
            isRotating = false;
        };
    }
    
    private void FixedUpdate() {
        // Check for movement input and move ship accordingly
        if (playerInputActions.PlayerShip.Movement.IsPressed()) {
            float value = playerInputActions.PlayerShip.Movement.ReadValue<float>();
            Move(value);
        }

        // Check for rotation input and rotate ship accordingly
        if (isRotating) {
            Rotate(new Vector3(
                playerInputActions.PlayerShip.RotateAlongX.ReadValue<float>(),
                playerInputActions.PlayerShip.RotateAlongY.ReadValue<float>(),
                playerInputActions.PlayerShip.RotateAlongZ.ReadValue<float>()
                ));
        }
        
        // Check if camera alignment is active, rotate if yes
        if (playerInputActions.PlayerShip.AlignWithCamera.IsPressed())
            AlignWithCamera();
    }

    // Move along Z axis (forward or backward)
    public void Move(float zAxisValue) {
        rb.AddRelativeForce(moveSpeed * new Vector3(0, 0, zAxisValue), ForceMode.Force);
    }

    // Rotate along the input vector
    public void Rotate(Vector3 inputVector) {
        rb.AddRelativeTorque(rotationSpeed * inputVector, ForceMode.Force);
    }

    // Rotate to align with the direction where camera is pointed at
    public void AlignWithCamera() {
        rb.MoveRotation
        (
            Quaternion.Slerp
            (
                transform.rotation, 
                mainCamera.transform.rotation, 
                cameraAlignRotationSpeed * Time.fixedDeltaTime
            )
        );
    }
}
