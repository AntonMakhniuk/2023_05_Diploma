using Assets.Scripts.Instruments;
using UnityEngine;

public class TractorBeam : Instrument
{
    public Texture2D crosshairTexture;
    public float tractorBeamRange = 10f;
    public float tractorSpeed = 5f;
    public float TractorBeamOffset { get; private set; }

    private bool isTractorBeamActive = false;
    
    private Renderer halfSphereMeshRenderer;
    private Collider halfSphereCollider;

    protected override void Awake() {
        base.Awake();
        GameObject halfSphere = transform.Find("Half-Sphere (TractorBeam)").gameObject;
        halfSphereCollider = halfSphere.GetComponent<Collider>();
        halfSphereMeshRenderer = halfSphere.GetComponent<MeshRenderer>();
        
        halfSphereMeshRenderer.enabled = false;
        halfSphereCollider.enabled = false;
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
    private void OnGUI() {
        if (isTractorBeamActive)
        {
            float size = 20f;
            GUI.DrawTexture(new Rect((Screen.width - size) / 2, (Screen.height - size) / 2, size, size), crosshairTexture);
        }
    }

    public override void Toggle() {
        base.Toggle();
        halfSphereMeshRenderer.enabled = !halfSphereMeshRenderer.enabled;
        halfSphereCollider.enabled = !halfSphereCollider.enabled;
    }
    //
    // public void ActivateTractorBeam()
    // {
    //     isTractorBeamActive = true;
    // }
    //
    // public void DeactivateTractorBeam()
    // {
    //     isTractorBeamActive = false;
    // }
}
