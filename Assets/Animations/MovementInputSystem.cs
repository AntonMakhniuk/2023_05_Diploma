using System.Collections;
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
    [SerializeField] private float rotationSpeed = 500f;
    [SerializeField] private float turnSpeed = 500f;
    [SerializeField] private float rotateAlongZSpeed = 500f;
    [SerializeField] private float deceleration = 5f;

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

        StartCoroutine(DecelerateCoroutine());
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
        isMovingForward = true;
    }

    private void StopMovingForward(InputAction.CallbackContext ctx)
    {
        isMovingForward = false;
    }

    private void StartMovingBackward(InputAction.CallbackContext ctx)
    {
        isMovingBackward = true;
    }

    private void StopMovingBackward(InputAction.CallbackContext ctx)
    {
        isMovingBackward = false;
    }

    private void StartRotatingLeft(InputAction.CallbackContext ctx)
    {
        isRotatingLeft = true;
    }

    private void StopRotatingLeft(InputAction.CallbackContext ctx)
    {
        isRotatingLeft = false;
    }

    private void StartRotatingRight(InputAction.CallbackContext ctx)
    {
        isRotatingRight = true;
    }

    private void StopRotatingRight(InputAction.CallbackContext ctx)
    {
        isRotatingRight = false;
    }

    private void StartTurningUp(InputAction.CallbackContext ctx)
    {
        isTurningUp = true;
    }

    private void StopTurningUp(InputAction.CallbackContext ctx)
    {
        isTurningUp = false;
    }

    private void StartTurningDown(InputAction.CallbackContext ctx)
    {
        isTurningDown = true;
    }

    private void StopTurningDown(InputAction.CallbackContext ctx)
    {
        isTurningDown = false;
    }

    private void StartRotatingAlongZLeft(InputAction.CallbackContext ctx)
    {
        isRotatingAlongZLeft = true;
    }

    private void StopRotatingAlongZLeft(InputAction.CallbackContext ctx)
    {
        isRotatingAlongZLeft = false;
    }

    private void StartRotatingAlongZRight(InputAction.CallbackContext ctx)
    {
        isRotatingAlongZRight = true;
    }

    private void StopRotatingAlongZRight(InputAction.CallbackContext ctx)
    {
        isRotatingAlongZRight = false;
    }

    private IEnumerator DecelerateCoroutine()
    {
        while (true)
        {
            yield return null;

            // Gradually decelerate forward movement
            if (!isMovingForward && ship.velocity.magnitude > 0)
            {
                ship.velocity -= ship.transform.forward * deceleration * Time.deltaTime;
            }

            // Gradually decelerate backward movement
            if (!isMovingBackward && ship.velocity.magnitude > 0)
            {
                ship.velocity += ship.transform.forward * deceleration * Time.deltaTime;
            }

            // Gradually decelerate rotation
            if (!isRotatingLeft && !isRotatingRight && ship.angularVelocity.magnitude > 0)
            {
                ship.angularVelocity -= ship.angularVelocity * deceleration * Time.deltaTime;
            }

            // Gradually decelerate turning
            if (!isTurningUp && !isTurningDown && ship.angularVelocity.magnitude > 0)
            {
                ship.angularVelocity -= ship.angularVelocity * deceleration * Time.deltaTime;
            }

            // Gradually decelerate rotation along Z-axis
            if (!isRotatingAlongZLeft && !isRotatingAlongZRight && ship.angularVelocity.magnitude > 0)
            {
                ship.angularVelocity -= ship.angularVelocity * deceleration * Time.deltaTime;
            }
        }
    }

    private void Update()
    {
        if (isMovingForward)
        {
            // Move the ship forward using Rigidbody
            MoveForward();
        }

        if (isMovingBackward)
        {
            // Move the ship backward using Rigidbody
            MoveBackward();
        }

        if (isRotatingLeft)
        {
            // Rotate the ship to the left using Rigidbody
            RotateLeft();
        }

        if (isRotatingRight)
        {
            // Rotate the ship to the right using Rigidbody
            RotateRight();
        }

        if (isTurningUp)
        {
            // Turn the ship up using Rigidbody
            TurnUp();
        }

        if (isTurningDown)
        {
            // Turn the ship down using Rigidbody
            TurnDown();
        }

        if (isRotatingAlongZLeft)
        {
            // Rotate the ship along Z-axis (left) using Rigidbody
            RotateAlongZLeft();
        }

        if (isRotatingAlongZRight)
        {
            // Rotate the ship along Z-axis (right) using Rigidbody
            RotateAlongZRight();
        }
    }

    private void MoveForward()
    {
        // Move the ship forward using Rigidbody with adjustable speed
        ship.position += transform.forward * Time.deltaTime * forwardSpeed;
    }

    private void MoveBackward()
    {
        // Move the ship backward using Rigidbody with adjustable speed
        ship.position -= transform.forward * Time.deltaTime * backwardSpeed;
    }

    private void RotateLeft()
    {
        // Rotate the ship to the left using Rigidbody with adjustable speed
        ship.rotation *= Quaternion.Euler(Vector3.up * -Time.deltaTime * rotationSpeed);
    }

    private void RotateRight()
    {
        // Rotate the ship to the right using Rigidbody with adjustable speed
        ship.rotation *= Quaternion.Euler(Vector3.up * Time.deltaTime * rotationSpeed);
    }

    private void TurnUp()
    {
        // Turn the ship up using Rigidbody with adjustable speed
        ship.rotation *= Quaternion.Euler(Vector3.right * -Time.deltaTime * turnSpeed);
    }

    private void TurnDown()
    {
        // Turn the ship down using Rigidbody with adjustable speed
        ship.rotation *= Quaternion.Euler(Vector3.right * Time.deltaTime * turnSpeed);
    }

    private void RotateAlongZLeft()
    {
        // Rotate the ship along Z-axis (left) using Rigidbody with adjustable speed
        ship.rotation *= Quaternion.Euler(Vector3.forward * Time.deltaTime * rotateAlongZSpeed);
    }

    private void RotateAlongZRight()
    {
        // Rotate the ship along Z-axis (right) using Rigidbody with adjustable speed
        ship.rotation *= Quaternion.Euler(Vector3.forward * -Time.deltaTime * rotateAlongZSpeed);
    }
}
