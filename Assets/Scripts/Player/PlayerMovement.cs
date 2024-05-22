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
    private bool isInRotationMode = false;

    [Header("Ship Movement Parameters")]
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float accelerationDrag = 0f;
    [SerializeField] private float brakesDrag = 1.5f;
    [SerializeField] private float maxLinearVelocity = 1000;
    [Space]
    [Header("Ship Rotation Parameters")]
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float maxAngularVelocity = 1;
    [SerializeField] private float accelerationAngularDrag = 0.5f;
    [SerializeField] private float brakesAngularDrag = 1f;
    [SerializeField] private float cameraAlignRotationSpeed = 1f;
    
    private void Awake() 
    {
        playerInputActions = new PlayerInputActions();
        
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;
        rb.maxLinearVelocity = maxLinearVelocity;

        rb.drag = accelerationDrag;
        rb.angularDrag = accelerationAngularDrag;
        
        mainCamera = Camera.main;
        
        playerInputActions.Enable(); 
        
        // Initial movement mode setup
        playerInputActions.PlayerShip.Movement.Enable();
        playerInputActions.PlayerShip.RotateAlongX.Disable();
        playerInputActions.PlayerShip.RotateAlongY.Disable();
        playerInputActions.PlayerShip.RotateAlongZ.Disable();
        
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
        
        playerInputActions.PlayerShip.RotateAlongX.performed += _ => isRotating = true;
        playerInputActions.PlayerShip.RotateAlongX.canceled += _ => isRotating = false;
        
        playerInputActions.PlayerShip.RotateAlongY.performed += _ => isRotating = true;
        playerInputActions.PlayerShip.RotateAlongY.canceled += _ => isRotating = false;
        
        playerInputActions.PlayerShip.RotateAlongZ.performed += _ => isRotating = true;
        playerInputActions.PlayerShip.RotateAlongZ.canceled += _ => isRotating = false;

        playerInputActions.PlayerShip.ToggleMovementMode.performed += _ => ChangeMovementMode();
    }

    private void FixedUpdate()
    {
        // Check for movement input and move ship accordingly
        Move(playerInputActions.PlayerShip.Movement.ReadValue<Vector3>());

        // Check for rotation input and rotate ship accordingly
        if (isRotating)
        {
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

    private void Move(Vector3 movementVector) 
    {
        rb.AddRelativeForce(moveSpeed * new Vector3(movementVector.x, movementVector.y, movementVector.z), ForceMode.Force);
    }

    // Rotate along the input vector
    private void Rotate(Vector3 inputVector) 
    {
        rb.AddRelativeTorque(rotationSpeed * inputVector, ForceMode.Force);
    }

    // Rotate to align with the direction where camera is pointed at
    private void AlignWithCamera() 
    {
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

    private void ChangeMovementMode()
    {
        Debug.Log("Changed");
        isInRotationMode = !isInRotationMode;
        if (isInRotationMode)
        {
            playerInputActions.PlayerShip.Movement.Disable();
            playerInputActions.PlayerShip.RotateAlongX.Enable();
            playerInputActions.PlayerShip.RotateAlongY.Enable();
            playerInputActions.PlayerShip.RotateAlongZ.Enable();
        }
        else
        {
            playerInputActions.PlayerShip.Movement.Enable();
            playerInputActions.PlayerShip.RotateAlongX.Disable();
            playerInputActions.PlayerShip.RotateAlongY.Disable();
            playerInputActions.PlayerShip.RotateAlongZ.Disable();
        }
    }
}
