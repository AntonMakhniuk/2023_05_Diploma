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

    private FuelSystem fuelSystem;
    private float fuelConsumptionRate = 0.5f;



    // Required for detecting rotation (no need for that for movement since only rotation is read as 3D Vector)
    private bool isRotating = false;

    [Header("Ship Movement Parameters")]
    [SerializeField] private float _moveSpeed = 1;
    [SerializeField] private float _accelerationDrag = 0f;
    [SerializeField] private float _brakesDrag = 1.5f;
    [SerializeField] private float _maxLinearVelocity = 1000;
    [Space]
    [Header("Ship Rotation Parameters")]
    [SerializeField] private float _rotationSpeed = 1;
    [SerializeField] private float _maxAngularVelocity = 1;
    [SerializeField] private float _accelerationAngularDrag = 0.5f;
    [SerializeField] private float _brakesAngularDrag = 1f;
    [SerializeField] private float _cameraAlignRotationSpeed = 1f;

    [SerializeField] private float _speedBoostMultiplier = 2f;
    [SerializeField] private float _speedBoostDuration = 10f;
    private bool isSpeedBoosted = false;
    private int speedBoostsCount = 0;


    private void Awake() {
        playerInputActions = new PlayerInputActions();
        
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = _maxAngularVelocity;
        rb.maxLinearVelocity = _maxLinearVelocity;

        rb.drag = _accelerationDrag;
        rb.angularDrag = _accelerationAngularDrag;
        
        mainCamera = Camera.main;
        
        playerInputActions.Enable();

        fuelSystem = GetComponent<FuelSystem>();

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
            rb.drag = _brakesDrag;
            rb.angularDrag = _brakesAngularDrag;
        };

        playerInputActions.PlayerShip.Brakes.canceled += _ =>
        {
            rb.drag = _accelerationDrag;
            rb.angularDrag = _accelerationAngularDrag;
        };

        // Y axis
        playerInputActions.PlayerShip.RotateAlongY.performed += _ => isRotating = true;
        
        playerInputActions.PlayerShip.RotateAlongY.canceled += _ => isRotating = false;

        // Z axis
        playerInputActions.PlayerShip.RotateAlongZ.performed += _ => isRotating = true;
        
        playerInputActions.PlayerShip.RotateAlongZ.canceled += _ => isRotating = false;

        StartCoroutine(ConsumeFuel());

    }

    public void SpeedBoost()
    {
        StartCoroutine(ApplySpeedBoost());
    }

    private IEnumerator ApplySpeedBoost()
    {
        speedBoostsCount++;

        _moveSpeed *= _speedBoostMultiplier;
        _rotationSpeed *= _speedBoostMultiplier;

        isSpeedBoosted = true;

        yield return new WaitForSeconds(_speedBoostDuration);

        _moveSpeed /= _speedBoostMultiplier;
        _rotationSpeed /= _speedBoostMultiplier;

        speedBoostsCount--;

        if (speedBoostsCount == 0)
        {
            isSpeedBoosted = false;
        }
    }



    private void FixedUpdate() {
        // Check for movement input and move ship accordingly
        if (playerInputActions.PlayerShip.Movement.IsPressed()) {
            //float value = playerInputActions.PlayerShip.Movement.ReadValue<float>();
            Move(playerInputActions.PlayerShip.Movement.ReadValue<Vector2>());
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
        rb.AddRelativeForce(_moveSpeed * new Vector3(0, movementVector.y, movementVector.x), ForceMode.Force);
    }

    // Rotate along the input vector
    public void Rotate(Vector3 inputVector) {
        rb.AddRelativeTorque(_rotationSpeed * inputVector, ForceMode.Force);
    }

    // Rotate to align with the direction where camera is pointed at
    public void AlignWithCamera() {
        rb.MoveRotation
        (
            Quaternion.Slerp
            (
                transform.rotation, 
                mainCamera.transform.rotation,
                _cameraAlignRotationSpeed * Time.fixedDeltaTime
            )
        );
    }

    private IEnumerator ConsumeFuel()
    {
        while (true)
        {
            yield return null;

            // Consume fuel when moving
            if (playerInputActions.PlayerShip.Movement.IsPressed())
            {
                fuelSystem.ConsumeFuel(fuelConsumptionRate * Time.deltaTime);
                bool isFuelThresholdReached = fuelSystem.GetCurrentFuelLevel() > fuelSystem.GetMaxFuelCapacity() * fuelSystem.GetLowFuelThreshold();
                if (!isFuelThresholdReached)
                {
                    UpdateMovementSpeedsForLowFuel();
                    if (fuelSystem.GetCurrentFuelLevel() <= 1)
                    {
                        enabled = false;
                    }
                }
                else if (!isSpeedBoosted)
                {
                    _moveSpeed = 1;
                    _rotationSpeed = 1;
                }
            }
        }
    }
    public void UpdateMovementSpeedsForLowFuel()
    {
        _moveSpeed /= _speedBoostMultiplier;
        _rotationSpeed /= _speedBoostMultiplier;
    }
}

