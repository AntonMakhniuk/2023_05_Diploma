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
    [SerializeField] private float brakesDrag = 1.5f;
    [Space]
    [Header("Ship Rotation Parameters")]
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float maxAngularVelocity = 1;
    [SerializeField] private float accelerationAngularDrag = 0.1f;
    [SerializeField] private float brakesAngularDrag = 1f;
    [SerializeField] private float cameraAlignRotationSpeed = 1f;
    
    private void Awake() {
        playerInputActions = new PlayerInputActions();
        
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;

        rb.drag = accelerationDrag;
        rb.angularDrag = accelerationAngularDrag;
        
        mainCamera = Camera.main;
        
        playerInputActions.Enable(); 
        
        // If the player presses the movement key, the drag will be changed to allow for higher velocity to be reached
        // playerInputActions.PlayerShip.Movement.performed += context => { rb.drag = accelerationDrag; };
        // If the player stops pressing the movement key, the drag will be changed to rapidly decelerate the ship
        // playerInputActions.PlayerShip.Movement.canceled += context => { rb.drag = decelerationDrag; };

        // If the player presses the rotation key, the drag will be changed to allow for quicker turn speed
        // If the player stops pressing the rotation key, the drag will be changed to rapidly decelerate the ship's rotation
        // This is repeated for every axis
        
        // Brakes
        playerInputActions.PlayerShip.Brakes.performed += _ =>
        {
            rb.drag = brakesDrag;
            rb.angularDrag = brakesAngularDrag;
        };

        playerInputActions.PlayerShip.Brakes.canceled += _ =>
        {
            rb.drag = accelerationDrag;
            rb.angularDrag = accelerationAngularDrag;
        };

        // X axis
        // playerInputActions.PlayerShip.RotateAlongX.performed += context =>
        // {
        //     rb.angularDrag = accelerationAngularDrag;
        //     isRotating = true;
        // };
        //
        // playerInputActions.PlayerShip.RotateAlongX.canceled += context =>
        // {
        //     rb.angularDrag = decelerationAngularDrag;
        //     isRotating = false;
        // };

        // Y axis
        playerInputActions.PlayerShip.RotateAlongY.performed += _ => isRotating = true;
        
        playerInputActions.PlayerShip.RotateAlongY.canceled += _ => isRotating = false;

        // Z axis
        playerInputActions.PlayerShip.RotateAlongZ.performed += _ => isRotating = true;
        
        playerInputActions.PlayerShip.RotateAlongZ.canceled += _ => isRotating = false;
    }
    
    private void FixedUpdate() {
        // Check for movement input and move ship accordingly
        if (playerInputActions.PlayerShip.Movement2.IsPressed()) {
            //float value = playerInputActions.PlayerShip.Movement.ReadValue<float>();
            Move(playerInputActions.PlayerShip.Movement2.ReadValue<Vector2>());
        }

        // Check for rotation input and rotate ship accordingly
        if (isRotating) {
            Rotate(new Vector3(
                //playerInputActions.PlayerShip.RotateAlongX.ReadValue<float>()
                0,
                playerInputActions.PlayerShip.RotateAlongY.ReadValue<float>(),
                playerInputActions.PlayerShip.RotateAlongZ.ReadValue<float>()
                ));
        }
        
        // Check if camera alignment is active, rotate if yes
        if (playerInputActions.PlayerShip.AlignWithCamera.IsPressed())
            AlignWithCamera();
    }

    // Move along Z axis (forward or backward)
    // public void Move(float zAxisValue) {
    //     rb.AddRelativeForce(moveSpeed * new Vector3(0, 0, zAxisValue), ForceMode.Force);
    // }
    
    // Move along Z and Y axis (forward or backward / up or down)
    public void Move(Vector2 movementVector) {
        rb.AddRelativeForce(moveSpeed * new Vector3(0, movementVector.y, movementVector.x), ForceMode.Force);
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
