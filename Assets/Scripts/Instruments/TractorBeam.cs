using Assets.Scripts.Instruments;
using Cinemachine;
using UnityEngine;

public class TractorBeam : Instrument
{
    public Texture2D crosshairTexture;
    public float tractorBeamRange = 10f;
    public float tractorSpeed = 5f;
    public float TractorBeamOffset { get; private set; }

    private bool isTractorBeamActive = false;
    
    private CinemachineFreeLook tractorBeamAimCamera;
    private Renderer barrelMeshRenderer;
    private Collider barrelCollider;
    private Transform barrelTransform;
    private Vector3 barrelDefaultPosition;

    private int cameraPriorityDiff = 10;

    protected override void Awake() {
        base.Awake();
        tractorBeamAimCamera = GetComponentInChildren<CinemachineFreeLook>();
        GameObject barrel = transform.Find("Barrel (TractorBeam)").gameObject;
        barrelCollider = barrel.GetComponent<Collider>();
        barrelMeshRenderer = barrel.GetComponent<MeshRenderer>();
        barrelTransform = barrel.transform;
        barrelDefaultPosition = barrelTransform.position;
        
        barrelMeshRenderer.enabled = false;
        barrelCollider.enabled = false;
    }

    // void Start()
    // {
    //     TractorBeamOffset = 0; // Set the default offset
    // }
    // void Update()
    // {
    //     if (isTractorBeamActive)
    //     {
    //         UpdateTractorBeamPosition();
    //         RotateTractorBeam();
    //     }
    //
    //     if (Input.GetMouseButton(0) && isTractorBeamActive)
    //     {
    //         PullObject();
    //     }
    //
    //     if (Input.GetMouseButtonDown(1) && isTractorBeamActive)
    //     {
    //         PushObjects();
    //     }
    // }
    //
    // void RotateTractorBeam()
    // {
    //     transform.rotation = transform.parent.rotation;
    // }
    //
    // void UpdateTractorBeamPosition()
    // {
    //     transform.position = transform.parent.position - transform.parent.up * TractorBeamOffset;
    // }
    //
    // void PullObject()
    // {
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     RaycastHit hit;
    //
    //     if (Physics.Raycast(ray, out hit, tractorBeamRange))
    //     {
    //         hit.rigidbody.AddForce((transform.parent.position - hit.transform.position).normalized * tractorSpeed);
    //     }
    // }
    //
    // void PushObjects()
    // {
    //     Collider[] hitColliders = Physics.OverlapSphere(transform.parent.position, tractorBeamRange);
    //     foreach (Collider collider in hitColliders)
    //     {
    //         Vector3 forceDirection = (collider.transform.position - transform.parent.position).normalized;
    //         collider.GetComponent<Rigidbody>().AddForce(forceDirection * tractorSpeed, ForceMode.Impulse);
    //     }
    // }
    //
    // private void OnGUI() {
    //     if (isTractorBeamActive)
    //     {
    //         float size = 20f;
    //         GUI.DrawTexture(new Rect((Screen.width - size) / 2, (Screen.height - size) / 2, size, size), crosshairTexture);
    //     }
    // }

    public override void Toggle() {
        base.Toggle();
        barrelTransform.position = barrelDefaultPosition;
        barrelMeshRenderer.enabled = !barrelMeshRenderer.enabled;
        barrelCollider.enabled = !barrelCollider.enabled;
        ChangeCamera();
    }

    private void ChangeCamera() {
        tractorBeamAimCamera.Priority += cameraPriorityDiff;
        cameraPriorityDiff *= -1;
    }
    
    public void ActivateTractorBeam()
    {
        isTractorBeamActive = true;
    }
    
    public void DeactivateTractorBeam()
    {
        isTractorBeamActive = false;
    }
}
