using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MovementInputSystem : MonoBehaviour
{
    private Rigidbody ship;
    private MovementActions movementActions;
    private FuelSystem fuelSystem;

    private float fuelConsumptionRate = 0.5f;


    private bool isMovingForward = false;
    private bool isMovingBackward = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isTurningUp = false;
    private bool isTurningDown = false;
    private bool isRotatingAlongZLeft = false;
    private bool isRotatingAlongZRight = false;

    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float backwardSpeed = 5f;
    [SerializeField] private float rotationSpeed = 25f;
    [SerializeField] private float turnSpeed = 50f;
    [SerializeField] private float rotateAlongZSpeed = 50f;
    [SerializeField] private float deceleration = 50f;

    [SerializeField] private float speedBoostMultiplier = 2f;
    [SerializeField] private float speedBoostDuration = 10f;
    private bool isSpeedBoosted = false;


    // private void OnTriggerEnter(Collider other)
    //{
    //  if (other.CompareTag("Accelerator"))
    //{
    //  StartCoroutine(ApplySpeedBoost());
    // You may want to disable the accelerator object after it's been used.
    //other.gameObject.SetActive(false);
    //}
    //}

    private int speedBoostsCount = 0;

    public void SpeedBoost()
    {
        StartCoroutine(ApplySpeedBoost());
    }

    private IEnumerator ApplySpeedBoost()
    {
        // Increment the speed boosts count
        speedBoostsCount++;

        // Apply speed boost
        forwardSpeed *= speedBoostMultiplier;
        backwardSpeed *= speedBoostMultiplier;
        rotationSpeed *= speedBoostMultiplier;
        turnSpeed *= speedBoostMultiplier;
        rotateAlongZSpeed *= speedBoostMultiplier;

        isSpeedBoosted = true;

        // Wait for the duration of the speed boost
        yield return new WaitForSeconds(speedBoostDuration);

        // Remove speed boost
        forwardSpeed /= speedBoostMultiplier;
        backwardSpeed /= speedBoostMultiplier;
        rotationSpeed /= speedBoostMultiplier;
        turnSpeed /= speedBoostMultiplier;
        rotateAlongZSpeed /= speedBoostMultiplier;

        // Decrement the speed boosts count
        speedBoostsCount--;

        // If no speed boosts are active, reset isSpeedBoosted flag
        if (speedBoostsCount == 0)
        {
            isSpeedBoosted = false;
        }
    }
    private void Awake()
    {
        ship = GetComponent<Rigidbody>();

        // Initialize fuel level
        //currentFuelLevel = maxFuelCapacity;
        fuelSystem = GetComponent<FuelSystem>(); // Get the FuelSystem component attached to the player ship


        // Initialize movement actions
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

        // Start the fuel consumption coroutine
        StartCoroutine(ConsumeFuel());
    }

    private void OnEnable()
    {
        movementActions.Enable();
    }

    private void OnDisable()
    {
        movementActions.Disable();
    }

    private IEnumerator ConsumeFuel()
    {
        while (true)
        {
            yield return null;

            // Consume fuel when moving
            if (isMovingForward || isMovingBackward)
            {
                fuelSystem.ConsumeFuel(fuelConsumptionRate * Time.deltaTime);
                bool isFuelThresholdReached = fuelSystem.GetCurrentFuelLevel() > fuelSystem.GetMaxFuelCapacity() * fuelSystem.GetLowFuelThreshold() ? true : false;
                if (!isFuelThresholdReached)
                {
                    UpdateMovementSpeedsForLowFuel();
                    if(fuelSystem.GetCurrentFuelLevel() <= 1)
                    {
                        enabled = false;
                    }
                } else if (!isSpeedBoosted){
                    forwardSpeed = 5f;
                    backwardSpeed = 5f;
                    rotationSpeed = 50f;
                    turnSpeed = 50f;
                    rotateAlongZSpeed = 50f;
                }
            }
        }
    }
    public void UpdateMovementSpeedsForLowFuel()
    {
        forwardSpeed = 2.5f;
        backwardSpeed = 2.5f;
        rotationSpeed = 25f;
        turnSpeed = 25f;
        rotateAlongZSpeed = 25f;
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

    private void Update()
    {
        if (isMovingForward)
        {
            MoveForward();
        }

        if (isMovingBackward)
        {
            MoveBackward();
        }

        if (isRotatingLeft)
        {
            RotateLeft();
        }

        if (isRotatingRight)
        {
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
