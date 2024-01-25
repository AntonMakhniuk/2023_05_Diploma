using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {
    private PlayerInputActions playerInputActions;
    private Rigidbody rb;

    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float accelerationDrag = 0.3f;
    [SerializeField] private float decelerationDrag = 1.5f;
    
    private void Awake() {
        playerInputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
        
        // Enable the action map
        playerInputActions.Enable();
        
        // If the player presses the movement key, the drag will be changed to allow for higher velocity to be reached
        playerInputActions.PlayerShip.Movement.performed += context => { rb.drag = accelerationDrag; };
        // If the player presses the movement key, the drag will be changed to rapidly decelerate the ship
        playerInputActions.PlayerShip.Movement.canceled += context => { rb.drag = decelerationDrag; };
    }

    private void FixedUpdate() {
        float value = playerInputActions.PlayerShip.Movement.ReadValue<float>();
        Move(value);
    }

    // Move along Z axis (forward or backward)
    public void Move(float zAxisValue) {
        rb.AddForce(moveSpeed * Time.deltaTime * new Vector3(0, 0, zAxisValue), ForceMode.Force);
    }

    public void Rotate(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            Debug.Log(ctx);
        }
    }
}
