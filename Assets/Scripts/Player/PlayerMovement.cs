using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {
    private PlayerInputActions playerInputActions;
    private Rigidbody rb;

    [Header("Ship Movement Parameters")]
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float accelerationDrag = 0.3f;
    [SerializeField] private float decelerationDrag = 1.5f;
    [Space]
    [Header("Ship Rotation Parameters")]
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float accelerationAngularDrag = 0.1f;
    [SerializeField] private float decelerationAngularDrag = 1f;
    
    private void Awake() {
        playerInputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
        
        // Enable the action map
        playerInputActions.Enable(); 
        
        // If the player presses the movement key, the drag will be changed to allow for higher velocity to be reached
        playerInputActions.PlayerShip.Movement.performed += context => { rb.drag = accelerationDrag; };
        // If the player presses the movement key, the drag will be changed to rapidly decelerate the ship
        playerInputActions.PlayerShip.Movement.canceled += context => { rb.drag = decelerationDrag; };

        // If the player presses the movement key, the drag will be changed to allow for quicker turn speed
        playerInputActions.PlayerShip.Rotation.performed += context => { rb.angularDrag = accelerationAngularDrag; };
        // If the player presses the movement key, the drag will be changed to rapidly decelerate the ship's rotation
        playerInputActions.PlayerShip.Rotation.performed += context => { rb.angularDrag = decelerationAngularDrag; };
    }
    
    private void FixedUpdate() {
        float value = playerInputActions.PlayerShip.Movement.ReadValue<float>();
        Move(value);

        Vector3 inputVector = playerInputActions.PlayerShip.Rotation.ReadValue<Vector3>();
        Rotate(inputVector);
    }

    // Move along Z axis (forward or backward)
    public void Move(float zAxisValue) {
        rb.AddRelativeForce(moveSpeed * new Vector3(0, 0, zAxisValue), ForceMode.Force);
    }

    public void Rotate(Vector3 inputVector) {
        rb.AddRelativeTorque(rotationSpeed * inputVector, ForceMode.Force);
    }
}
