using System;
using Assets.Scripts.Instruments;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class TractorBeam : Instrument
{
    [SerializeField] private float tractorBeamRange = 10f;
    private RaycastHit lastRaycastHit;
    [SerializeField] private float tractorSpeed = 5f;
    
    private CinemachineFreeLook tractorBeamAimCamera;
    private Camera mainCamera;
    private int cameraPriorityDiff = 10;
    private Vector3? defaultCameraPosition = null;
        //= new Vector3(-22, 4.5f, -3);
    private Quaternion? defaultCameraRotation = null;
        //= Quaternion.Euler(new Vector3(-53.13f, 1, 0));
    
    [SerializeField] private float halfSphereRotationSpeed = 10f;
    [SerializeField] private Canvas crosshairCanvas;
    [SerializeField] private LayerMask tractorBeamAimLayerMask;

    private PlayerInputActions playerInputActions;
    protected InventoryWindow inventory;
    
    private BoxCollider boxCollider;
    private Renderer barrelMeshRenderer;
    private Collider barrelCollider;
    private Transform barrelTransform;
    private Vector3 barrelDefaultPosition;
    private Quaternion halfSphereDefaultLocalRotation;

    protected override void Awake() {
        base.Awake();
        
        tractorBeamAimCamera = GetComponentInChildren<CinemachineFreeLook>();
        mainCamera = Camera.main;

        inventory = GetComponentInParent<InventoryWindow>();
        
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        
        // TODO: Is this required for half-sphere rotation?
        playerInputActions.PlayerShip.MouseCameraMovement.performed += _ =>
        {
            if (barrelMeshRenderer.enabled) {
                RotateHalfSphere();
            }
        };
        
        playerInputActions.PlayerShip.Movement.performed += _ =>
        {
            if (barrelMeshRenderer.enabled) {
                RotateHalfSphere();
            }
        };
        
        // playerInputActions.PlayerShip.RotateAlongX.performed += _ =>
        // {
        //     if (barrelMeshRenderer.enabled) {
        //         RotateHalfSphere();
        //     }
        // };
        
        playerInputActions.PlayerShip.RotateAlongY.performed += _ =>
        {
            if (barrelMeshRenderer.enabled) {
                RotateHalfSphere();
            }
        };
        
        playerInputActions.PlayerShip.RotateAlongZ.performed += _ =>
        {
            if (barrelMeshRenderer.enabled) {
                RotateHalfSphere();
            }
        };

        playerInputActions.PlayerShip.InstrumentSecondary.performed += _ => { PushObjects(); };
        
        GameObject barrel = transform.Find("Barrel (TractorBeam)").gameObject;
        barrelCollider = barrel.GetComponent<Collider>();
        barrelMeshRenderer = barrel.GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        
        barrelTransform = barrel.transform;
        barrelDefaultPosition = barrelTransform.localPosition;
        halfSphereDefaultLocalRotation = transform.localRotation;
        
        Debug.Log(halfSphereDefaultLocalRotation.eulerAngles);

        boxCollider.enabled = false;
        barrelMeshRenderer.enabled = false;
        barrelCollider.enabled = false;
        crosshairCanvas.enabled = false;
    }

    private void Update() {
        if (barrelMeshRenderer.enabled) {
            Vector2 crosshairPoint = new Vector2(Screen.width / 2f, Screen.height / 2f + 80);
            Ray ray = mainCamera.ScreenPointToRay(crosshairPoint);
        
            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, tractorBeamAimLayerMask)) {
                lastRaycastHit = raycastHit;
                
                if (Mouse.current.leftButton.IsPressed())
                    PullObject(raycastHit);
            }
        }
    }
    void PullObject(RaycastHit hit)
    {
        hit.rigidbody.AddForce(tractorSpeed * (transform.parent.position - hit.transform.position).normalized);
    }
    
    void PushObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.parent.position, tractorBeamRange, tractorBeamAimLayerMask);
        foreach (Collider collider in hitColliders)
        {
            Vector3 forceDirection = (collider.transform.position - transform.parent.position).normalized;
            collider.GetComponent<Rigidbody>().AddForce(forceDirection * tractorSpeed * 5f, ForceMode.Impulse);
        }
    }

    public override void Toggle() {
        base.Toggle();
        barrelMeshRenderer.enabled = !barrelMeshRenderer.enabled;
        barrelCollider.enabled = !barrelCollider.enabled;
        crosshairCanvas.enabled = !crosshairCanvas.enabled;
        boxCollider.enabled = !boxCollider.enabled;
        
        //barrelTransform.localPosition = barrelDefaultPosition;
        //transform.localRotation = halfSphereDefaultLocalRotation;
        
        ChangeCamera();
    }

    private void ChangeCamera() {
        tractorBeamAimCamera.Priority += cameraPriorityDiff;

        if (defaultCameraPosition == null) {
            defaultCameraPosition = mainCamera.transform.localPosition;
            defaultCameraRotation = mainCamera.transform.localRotation;
        }
        
        tractorBeamAimCamera.ForceCameraPosition(defaultCameraPosition.Value, defaultCameraRotation.Value);
        
        cameraPriorityDiff *= -1;
    }

    private void RotateHalfSphere() {
    	Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, mainCamera.transform.rotation.eulerAngles.y+180f, transform.rotation.eulerAngles.z);
        
    	transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
	}

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Resource")) {
            inventory.IncreaseSpaceOreQuantity(1);
            Destroy(other.gameObject);
        }
    }
}
