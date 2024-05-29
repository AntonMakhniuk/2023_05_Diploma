using Assets.Scripts.Instruments;
using Cinemachine;
using UnityEngine;

public class BombContainer : Instrument
{
    [SerializeField] private Transform bombBarrel;
    [SerializeField] private Transform bombBase;
    [SerializeField] private float rotationSpeed;
    
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Transform muzzlePoint;
    public float bombSpeed = 5f;
    [SerializeField] private float bombLifetime = 3f;
    [SerializeField] private float bombRange = 5f;
    [SerializeField] private CinemachineVirtualCamera cinematicCamera;
    [SerializeField] private Canvas crosshairCanvas;
    private int cameraPriorityDiff = 10;
    
    private PlayerInputActions _playerInputActions;
    private Transform _mainCamera;
    private Transform _spaceshipTransform; 
    

    private void Awake()
    {
        if (isActiveTool==false)
        {
            ToggleInstrument(false);
        }
        _playerInputActions = new PlayerInputActions();
        if (Camera.main != null) _mainCamera = Camera.main.transform;
        if (transform.root != null) _spaceshipTransform = transform.root;

        _playerInputActions.PlayerCamera.Enable();
    }
    

    void Update()
    {
        if (isActiveTool)
        {
            RotateWithCamera();
            Toggle();
            ChangeCamera();
            Work();
        }
        else
        {
            ToggleInstrument(false);
        }
    }
    
    public void RotateWithCamera() 
    {
        // Get the position of the laser base
        Vector3 laserBasePosition = bombBase.transform.position;

        // Get the position and forward direction of the cinemachine camera
        Vector3 cameraPosition = cinematicCamera.transform.position;
        Vector3 cameraForward = cinematicCamera.transform.forward;

        // Calculate the direction from the laser base to the camera's forward direction
        Vector3 direction = cameraPosition + cameraForward * 100f - laserBasePosition;

        // Transform the direction to be relative to the ship's rotation
        Vector3 relativeDirection = PlayerShip.Instance.transform.InverseTransformDirection(direction);
        
        // Calculate the rotation for the laser leg (Y-axis rotation)
        float legAngle = Mathf.Atan2(relativeDirection.x, relativeDirection.z) * Mathf.Rad2Deg;
        bombBase.transform.localRotation = Quaternion.Euler(0f, legAngle, 0f);
        

        // Calculate the rotation for the laser barrel (X-axis rotation)
        float barrelAngle = -Mathf.Atan2(relativeDirection.y, Mathf.Sqrt(relativeDirection.x * relativeDirection.x + relativeDirection.z * relativeDirection.z)) * Mathf.Rad2Deg;
        bombBarrel.transform.localRotation = Quaternion.Euler(barrelAngle, 0f, 0f);
    }

    void Work()
    {
        if (isActiveTool && Input.GetMouseButtonDown(0))
        {
            SpawnBomb();
        }

        if (isActiveTool && Input.GetMouseButtonDown(1))
        {
            DetonateAllBombs();
        }
    }

    void OnDrawGizmos()
    {
        if (isActiveTool)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, bombRange);
        }
    }

    void SpawnBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, muzzlePoint.position, muzzlePoint.rotation);
        
        Bomb bombScript = bomb.GetComponent<Bomb>();
        if (bombScript != null)
        {
            bombScript.SetBombContainer(this);
        }
        
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        rb.velocity = muzzlePoint.forward * bombSpeed;
        Destroy(bomb, bombLifetime);
    }

    void DetonateAllBombs()
    {
        Bomb[] bombs = FindObjectsOfType<Bomb>();
        foreach (Bomb bomb in bombs)
        {
            bomb.Detonate();
        }
    }

    public override void Toggle()
    {
        if (isActiveTool)
        {
            cinematicCamera.gameObject.SetActive(true);
            crosshairCanvas.gameObject.SetActive(true);
            SetActiveTool(true);
        }
        else
        {
            cinematicCamera.gameObject.SetActive(false);
            crosshairCanvas.gameObject.SetActive(false);
            SetActiveTool(false);
        }
        if (_spaceshipTransform != null)
        {
            transform.rotation = _spaceshipTransform.rotation;
        }
    }

    void ToggleInstrument(bool activate)
    {
        isActiveTool = activate;
        
        if (isActiveTool)
        {
            cinematicCamera.gameObject.SetActive(true);
            crosshairCanvas.gameObject.SetActive(true);
            SetActiveTool(true);
        }
        else
        {
            cinematicCamera.gameObject.SetActive(false);
            crosshairCanvas.gameObject.SetActive(false);
            SetActiveTool(false);
        }
        if (_spaceshipTransform != null)
        {
            transform.rotation = _spaceshipTransform.rotation;
        }
    }

    private void ChangeCamera() 
    {
        cinematicCamera.Priority += cameraPriorityDiff;
        cinematicCamera.Priority -= cameraPriorityDiff;
        
        if (cameraPriorityDiff < 0) 
        {
            cinematicCamera.transform.localPosition = Vector3.zero;
            cinematicCamera.transform.localRotation = Quaternion.identity;
        }

        cameraPriorityDiff *= -1;
    }
}