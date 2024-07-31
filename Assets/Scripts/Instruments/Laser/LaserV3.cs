using Assets.Scripts.Instruments;
using Cinemachine;
using Player;
using Player.Ship;
using UnityEngine;

public class LaserV3 : Instrument
{
    [SerializeField] private Transform laserBarrel;
    [SerializeField] private Transform laserLeg;
    [SerializeField] private Transform laserBase;
    [SerializeField] private float rotationSpeed;
    
    [SerializeField] private LineRenderer _beam;
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private float maxLenght;
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

        //_playerInputActions.PlayerCamera.Enable();
        
        _beam.enabled = false;
        _beam.startWidth = 0.1f;
        _beam.endWidth = 0.1f;
    }
    
    private void Activate()
    {
        _beam.enabled = true;
    }

    private void Deactivate()
    {
        _beam.enabled = false;
        var position = _muzzlePoint.position;
        _beam.SetPosition(0, position);
        _beam.SetPosition(1, position);
    }

    private void Update() {
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
    
    private void LateUpdate()
    {
        if (isActiveTool)
        {
            cinematicCamera.transform.position = _spaceshipTransform.position;
            cinematicCamera.transform.rotation = _spaceshipTransform.rotation;
        }
    }

    void Work()
    {
        if (isActiveTool && Input.GetMouseButtonDown(0))
        {
            Activate();
        }
        else if (isActiveTool && Input.GetMouseButtonUp(0))
        {
            Deactivate();
        }
    }
    
    private void FixedUpdate()
    {
        if (!_beam.enabled)
        {
            return;
        }

        Ray ray = new Ray(_muzzlePoint.position, _muzzlePoint.forward);
        bool cast = Physics.Raycast(ray, out RaycastHit hit, maxLenght);
        Vector3 hitPosition = cast ? hit.point : _muzzlePoint.position + _muzzlePoint.forward * maxLenght;
        _beam.SetPosition(0, _muzzlePoint.position);
        _beam.SetPosition(1,hitPosition);
        if (cast && hit.collider.CompareTag("AsteroidPoint"))
        {
            CheckAndDestroyAsteroidPoint(hit.collider);
        }
    }

    private void CheckAndDestroyAsteroidPoint(Collider collider)
    {
        if (collider.CompareTag("AsteroidPoint"))
        {
            Destroy(collider.gameObject);

            Asteroid asteroid = collider.transform.parent.parent.GetComponent<Asteroid>();
            asteroid.OnAsteroidPointDestroyed();
        }
    }

    public void RotateWithCamera() 
    {
        // Get the position of the laser base
        Vector3 laserBasePosition = laserBase.transform.position;

        // Get the position and forward direction of the cinemachine camera
        Vector3 cameraPosition = cinematicCamera.transform.position;
        Vector3 cameraForward = cinematicCamera.transform.forward;

        // Calculate the direction from the laser base to the camera's forward direction
        Vector3 direction = cameraPosition + cameraForward * 100f - laserBasePosition;

        // Transform the direction to be relative to the ship's rotation
        Vector3 relativeDirection = PlayerShip.Instance.transform.InverseTransformDirection(direction);

        // Calculate the rotation for the laser leg (Y-axis rotation)
        float legAngle = Mathf.Atan2(relativeDirection.y, relativeDirection.z) * Mathf.Rad2Deg;
        laserLeg.transform.localRotation = Quaternion.Euler(0f, legAngle * -1, 0f);

        // Calculate the rotation for the laser barrel (X-axis rotation)
        float barrelAngle = -Mathf.Atan2(relativeDirection.x, Mathf.Sqrt(relativeDirection.y * relativeDirection.y + relativeDirection.z * relativeDirection.z)) * Mathf.Rad2Deg;
        laserBarrel.transform.localRotation = Quaternion.Euler(barrelAngle , 0f, 0f);
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
