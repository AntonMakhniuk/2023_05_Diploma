using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementInputSystem : MonoBehaviour
{
    private Rigidbody ship;
    private MovementActions movementActions;

    private bool isMovingForward = false;
    private bool isMovingBackward = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isTurningUp = false;
    private bool isTurningDown = false;
    private bool isRotatingAlongZLeft = false;
    private bool isRotatingAlongZRight = false;

    // Declare movement speed variables

    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float backwardSpeed = 5f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float turnSpeed = 50f;
    [SerializeField] private float rotateAlongZSpeed = 50f;

    private void Awake()
    {
        ship = GetComponent<Rigidbody>();
        ship.isKinematic = true; // Set Rigidbody to kinematic

        movementActions = new MovementActions();
        movementActions.Ship.Forward.started += StartMovingForward;
        movementActions.Ship.Forward.canceled += StopMovingForward;
        movementActions.Ship.Back.started += StartMovingBackward;
        movementActions.Ship.Back.canceled += StopMovingBackward;
        movementActions.Ship.RotateLeft.started += StartRotatingLeft;
        movementActions.Ship.RotateLeft.canceled += StopRotatingLeft;
        movementActions.Ship.RotateRight.started += StartRotatingRight;
        movementActions.Ship.RotateRight.canceled += StopRotatingRight;
        movementActions.Ship.RotateUp.started += StartTurningUp;
        movementActions.Ship.RotateUp.canceled += StopTurningUp;
        movementActions.Ship.RotateDown.started += StartTurningDown;
        movementActions.Ship.RotateDown.canceled += StopTurningDown;
        movementActions.Ship.RotateAlongZLeft.started += StartRotatingAlongZLeft;
        movementActions.Ship.RotateAlongZLeft.canceled += StopRotatingAlongZLeft;
        movementActions.Ship.RotateAlongZRight.started += StartRotatingAlongZRight;
        movementActions.Ship.RotateAlongZRight.canceled += StopRotatingAlongZRight;
    }

    private void OnEnable()
    {
        movementActions.Enable();
    }

    private void OnDisable()
    {
        movementActions.Disable();
    }

    private void StartMovingForward(InputAction.CallbackContext ctx)
    {
        // Button for moving forward pressed, start moving forward
        Debug.Log("W - Pressed");
        isMovingForward = true;
    }

    private void StopMovingForward(InputAction.CallbackContext ctx)
    {
        // Button for moving forward released, stop moving forward
        Debug.Log("W - Released");
        isMovingForward = false;
    }

    private void StartMovingBackward(InputAction.CallbackContext ctx)
    {
        // Button for moving backward pressed, start moving backward
        Debug.Log("S - Pressed");
        isMovingBackward = true;
    }

    private void StopMovingBackward(InputAction.CallbackContext ctx)
    {
        // Button for moving backward released, stop moving backward
        Debug.Log("S - Released");
        isMovingBackward = false;
    }

    private void StartRotatingLeft(InputAction.CallbackContext ctx)
    {
        // Button for rotating left pressed, start rotating left
        Debug.Log("A - Pressed");
        isRotatingLeft = true;
    }

    private void StopRotatingLeft(InputAction.CallbackContext ctx)
    {
        // Button for rotating left released, stop rotating left
        Debug.Log("A - Released");
        isRotatingLeft = false;
    }

    private void StartRotatingRight(InputAction.CallbackContext ctx)
    {
        // Button for rotating right pressed, start rotating right
        Debug.Log("D - Pressed");
        isRotatingRight = true;
    }

    private void StopRotatingRight(InputAction.CallbackContext ctx)
    {
        // Button for rotating right released, stop rotating right
        Debug.Log("D - Released");
        isRotatingRight = false;
    }

    private void StartTurningUp(InputAction.CallbackContext ctx)
    {
        // Button for turning up pressed, start turning up
        Debug.Log("Q - Pressed");
        isTurningUp = true;
    }

    private void StopTurningUp(InputAction.CallbackContext ctx)
    {
        // Button for turning up released, stop turning up
        Debug.Log("Q - Released");
        isTurningUp = false;
    }

    private void StartTurningDown(InputAction.CallbackContext ctx)
    {
        // Button for turning down pressed, start turning down
        Debug.Log("E - Pressed");
        isTurningDown = true;
    }

    private void StopTurningDown(InputAction.CallbackContext ctx)
    {
        // Button for turning down released, stop turning down
        Debug.Log("E - Released");
        isTurningDown = false;
    }

    private void StartRotatingAlongZLeft(InputAction.CallbackContext ctx)
    {
        // Button for rotating along Z-axis (left) pressed, start rotating along Z-axis (left)
        Debug.Log("Z - Pressed");
        isRotatingAlongZLeft = true;
    }

    private void StopRotatingAlongZLeft(InputAction.CallbackContext ctx)
    {
        // Button for rotating along Z-axis (left) released, stop rotating along Z-axis (left)
        Debug.Log("Z - Released");
        isRotatingAlongZLeft = false;
    }

    private void StartRotatingAlongZRight(InputAction.CallbackContext ctx)
    {
        // Button for rotating along Z-axis (right) pressed, start rotating along Z-axis (right)
        Debug.Log("C - Pressed");
        isRotatingAlongZRight = true;
    }

    private void StopRotatingAlongZRight(InputAction.CallbackContext ctx)
    {
        // Button for rotating along Z-axis (right) released, stop rotating along Z-axis (right)
        Debug.Log("C - Released");
        isRotatingAlongZRight = false;
    }

    private void Update()
    {
        if (isMovingForward)
        {
            // Move the ship forward directly (kinematic)
            MoveForward();
        }

        if (isMovingBackward)
        {
            // Move the ship backward directly (kinematic)
            MoveBackward();
        }

        if (isRotatingLeft)
        {
            // Rotate the ship to the left directly (kinematic)
            RotateLeft();
        }

        if (isRotatingRight)
        {
            // Rotate the ship to the right directly (kinematic)
            RotateRight();
        }

        if (isTurningUp)
        {
            // Turn the ship up directly (kinematic)
            TurnUp();
        }

        if (isTurningDown)
        {
            // Turn the ship down directly (kinematic)
            TurnDown();
        }

        if (isRotatingAlongZLeft)
        {
            // Rotate the ship along Z-axis (left) directly (kinematic)
            RotateAlongZLeft();
        }

        if (isRotatingAlongZRight)
        {
            // Rotate the ship along Z-axis (right) directly (kinematic)
            RotateAlongZRight();
        }
    }

    private void MoveForward()
    {
        // Move the ship forward directly (kinematic) with adjustable speed
        transform.Translate(Time.deltaTime * forwardSpeed * Vector3.forward);
    }

    private void MoveBackward()
    {
        // Move the ship backward directly (kinematic) with adjustable speed
        transform.Translate(Time.deltaTime * backwardSpeed * Vector3.back);
    }

    private void RotateLeft()
    {
        // Rotate the ship to the left directly (kinematic) with adjustable speed
        transform.Rotate(Vector3.up, -Time.deltaTime * rotationSpeed);
    }

    private void RotateRight()
    {
        // Rotate the ship to the right directly (kinematic) with adjustable speed
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
    }

    private void TurnUp()
    {
        // Turn the ship up directly (kinematic) with adjustable speed
        transform.Rotate(Vector3.right, -Time.deltaTime * turnSpeed);
    }

    private void TurnDown()
    {
        // Turn the ship down directly (kinematic) with adjustable speed
        transform.Rotate(Vector3.right, Time.deltaTime * turnSpeed);
    }

    private void RotateAlongZLeft()
    {
        // Rotate the ship along Z-axis (left) directly (kinematic) with adjustable speed
        transform.Rotate(Vector3.forward, Time.deltaTime * rotateAlongZSpeed);
    }

    private void RotateAlongZRight()
    {
        // Rotate the ship along Z-axis (right) directly (kinematic) with adjustable speed
        transform.Rotate(Vector3.forward, -Time.deltaTime * rotateAlongZSpeed);
    }
}
