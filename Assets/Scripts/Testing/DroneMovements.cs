using Player;
using UnityEngine;

namespace Testing
{
    [RequireComponent(typeof(Rigidbody))]
    public class DroneMovements : MonoBehaviour
    {
        public static DroneMovements Instance;
        
        [SerializeField] private float thrustSpeed = 50f;
        [SerializeField] private float strafeSpeed = 30f;
        [SerializeField] private float boostMultiplier = 2f;
        
        
        private Rigidbody _rb;
        
        private PlayerInputActions _playerInputActions;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _playerInputActions = PlayerActions.InputActions;
            _rb = GetComponent<Rigidbody>();
            _playerInputActions.PlayerShip.Enable();
        }

        private void FixedUpdate()
        {
            if (_playerInputActions.PlayerShip.Thrust.IsPressed())
            {
                Move(new Vector2(_playerInputActions.PlayerShip.Thrust.ReadValue<float>(), 0));
            }
            if (_playerInputActions.PlayerShip.Yaw.IsPressed())
            {
                Move(new Vector2(0, _playerInputActions.PlayerShip.Yaw.ReadValue<float>()));
            }
        }


        private void Move(Vector2 movementVector) 
        {
            _rb.AddRelativeForce(thrustSpeed * boostMultiplier * Time.deltaTime 
                                 * new Vector3(movementVector.y, 0, movementVector.x), ForceMode.Force);
        }

    }

}
