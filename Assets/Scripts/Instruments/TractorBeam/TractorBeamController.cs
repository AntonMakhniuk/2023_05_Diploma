using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Instruments;
using Cinemachine;
using UnityEngine;

public class TractorBeamController : Instrument
{
    public float attractSpeed = 10f;
    public float holdDistance = 2f;
    public float pushForce = 20f;
    public float maxDistance = 100f;
    public Transform holdPoint;
    public LayerMask attractableLayer;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private CinemachineVirtualCamera cinematicCamera;
    [SerializeField] private Canvas crosshairCanvas;
    
    [SerializeField] private Transform beamBarrel;
    [SerializeField] private Transform beamLeg;
    [SerializeField] private Transform beamBase;
    
    private int cameraPriorityDiff = 10;

    private PlayerInputActions playerInputActions;
    private Rigidbody attractedObject;
    private ITractorBeamState currentState;
    private Transform _spaceshipTransform; 

    private void Awake()
    {
        if (isActiveTool==false)
        {
            ToggleInstrument(false);
        }
        
        currentState = new IdleState();
        playerInputActions = new PlayerInputActions();
        
        playerInputActions.PlayerShip.InstrumentPrimary.started += _ => OnPrimaryAction();
        playerInputActions.PlayerShip.InstrumentPrimary.canceled += _ => SetState(new IdleState());
        playerInputActions.PlayerShip.InstrumentSecondary.performed += _ => OnSecondaryAction();
        //playerInputActions.PlayerShip.InstrumentThird.performed += _ => OnThirdAction();
        
        
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    private void Update()
    {
        if (isActiveTool)
        {
            currentState.UpdateState(this);
            RotateWithCamera();
            Toggle();
            ChangeCamera();
        }
        else 
        {
            ToggleInstrument(false);
        }
    }

    public void SetState(ITractorBeamState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);
    }

    public void SetAttractedObject(Rigidbody obj)
    {
        attractedObject = obj;
    }

    public Rigidbody GetAttractedObject()
    {
        return attractedObject;
    }

    private void OnPrimaryAction()
    {
        if (currentState is IdleState)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, attractableLayer))
            {
                Rigidbody rb = hit.rigidbody;
                if (rb != null)
                {
                    SetAttractedObject(rb);
                    SetState(new AttractingState());
                }
            }
        }
    }

    private void OnSecondaryAction()
    {
        if (currentState is HoldingState)
        {
            SetState(new PushState());
        }
    }

    private void OnThirdAction()
    {
        if (currentState is HoldingState)
        {
            SetAttractedObject(null);
            SetState(new IdleState());
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
    public void RotateWithCamera() 
    {
        // Get the position of the laser base
        Vector3 laserBasePosition = beamBase.transform.position;

        // Get the position and forward direction of the cinemachine camera
        Vector3 cameraPosition = cinematicCamera.transform.position;
        Vector3 cameraForward = cinematicCamera.transform.forward;

        // Calculate the direction from the laser base to the camera's forward direction
        Vector3 direction = cameraPosition + cameraForward * 100f - laserBasePosition;

        // Transform the direction to be relative to the ship's rotation
        Vector3 relativeDirection = PlayerShip.Instance.transform.InverseTransformDirection(direction);

        // Calculate the rotation for the laser leg (Y-axis rotation)
        float legAngle = Mathf.Atan2(relativeDirection.x, relativeDirection.z) * Mathf.Rad2Deg;
        beamLeg.transform.localRotation = Quaternion.Euler(0f, legAngle * -1, 0f);

        // Calculate the rotation for the laser barrel (X-axis rotation)
        float barrelAngle = -Mathf.Atan2(relativeDirection.y, Mathf.Sqrt(relativeDirection.x * relativeDirection.x + relativeDirection.z * relativeDirection.z)) * Mathf.Rad2Deg;
        beamBarrel.transform.localRotation = Quaternion.Euler(barrelAngle, 0f * -1, 0f);
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
