using UnityEngine;
using UnityEngine.SceneManagement;
using Wagons.Systems;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour 
    {
        private PlayerInputActions playerInputActions;
        private Rigidbody rb;
        private Camera mainCamera;

        private bool isRotating = false;
        private bool isPitchingX = false;
        private bool isYawingY = false;
        private bool isRollingZ = false;

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
    
        private void Awake()
        {
            playerInputActions = PlayerActions.InputActions;
            playerInputActions.PlayerShip.Enable();
        
            rb = GetComponent<Rigidbody>();
            rb.maxAngularVelocity = maxAngularVelocity;

            rb.drag = accelerationDrag;
            rb.angularDrag = accelerationAngularDrag;
        
            // TODO: return it to how it was (without the null check)
            if (WagonManager.Instance != null) 
                WagonManager.Instance.SetDragValuesForAttachedWagons(accelerationDrag, accelerationAngularDrag);
        
            mainCamera = Camera.main;
        
            // Brakes
            playerInputActions.PlayerShip.Brakes.performed += _ =>
            {
                rb.drag = brakesDrag;
                rb.angularDrag = brakesAngularDrag;
            
                // TODO: return this back to how it was (without the null check)
                if (WagonManager.Instance != null) 
                    WagonManager.Instance.SetDragValuesForAttachedWagons(brakesDrag, brakesAngularDrag);
            };

            playerInputActions.PlayerShip.Brakes.canceled += _ =>
            {
                rb.drag = accelerationDrag;
                rb.angularDrag = accelerationAngularDrag;
            
                // TODO: return this back to how it was (without the null check)
                if (WagonManager.Instance != null) 
                    WagonManager.Instance.SetDragValuesForAttachedWagons(accelerationDrag, accelerationAngularDrag);
            };

            // Pitch (X axis)
            playerInputActions.PlayerShip.Pitch.performed += _ => isPitchingX = true;
            playerInputActions.PlayerShip.Pitch.canceled += _ => isPitchingX = false;
        
            // Yaw (Y axis)
            playerInputActions.PlayerShip.Yaw.performed += _ => isYawingY = true;
            playerInputActions.PlayerShip.Yaw.canceled += _ => isYawingY = false;
        
            // Roll (Z axis)
            playerInputActions.PlayerShip.Roll.performed += _ =>  isRollingZ = true;
            playerInputActions.PlayerShip.Roll.canceled += _ => isRollingZ = false;

            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void HandleSceneLoaded(Scene _, LoadSceneMode __)
        {
            mainCamera = Camera.main;
        }

        private void FixedUpdate() 
        {
            if (playerInputActions.PlayerShip.Thrust.IsPressed())
                Move(new Vector2(playerInputActions.PlayerShip.Thrust.ReadValue<float>(), 0));
        
            if (playerInputActions.PlayerShip.Strafe.IsPressed())
                Move(new Vector2(0, playerInputActions.PlayerShip.Strafe.ReadValue<float>()));
        
            if (isPitchingX)
                Rotate(new Vector3(playerInputActions.PlayerShip.Pitch.ReadValue<float>(), 0, 0));
        
            if (isYawingY)
                Rotate(new Vector3(0, playerInputActions.PlayerShip.Yaw.ReadValue<float>(), 0));
        
            if (isRollingZ)
                Rotate(new Vector3(0, 0, playerInputActions.PlayerShip.Roll.ReadValue<float>()/2f));
        
            // Check if camera alignment is active, rotate if yes
            if (playerInputActions.PlayerShip.AlignWithCamera.IsPressed())
                AlignWithCamera();
        }
    
        // Move along Z and Y axis (forward or backward / up or down)
        private void Move(Vector2 movementVector) 
        {
            rb.AddRelativeForce(moveSpeed * Time.deltaTime * new Vector3(movementVector.y, 0, movementVector.x), ForceMode.Force);
        }

        // Rotate along the input vector
        private void Rotate(Vector3 inputVector) 
        {
            rb.AddRelativeTorque(rotationSpeed * Time.deltaTime * inputVector, ForceMode.Force);
        }

        // Rotate to align with the direction where camera is pointed at
        private void AlignWithCamera()
        {
            rb.MoveRotation(
                Quaternion.Slerp(
                    transform.rotation, 
                    mainCamera.transform.rotation, 
                    cameraAlignRotationSpeed * Time.deltaTime
                )
            );
        
            // Funny
            // rb.MoveRotation(rb.rotation * Quaternion.Euler(
            //     Quaternion.FromToRotation(
            //         transform.rotation.eulerAngles, 
            //         mainCamera.transform.rotation.eulerAngles
            //         ).eulerAngles 
            //     * Time.fixedDeltaTime)
            // );
        }
    }
}
