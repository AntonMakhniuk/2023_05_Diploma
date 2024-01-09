using System.Collections;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    public GameObject tractorBeamPrefab; // The Tractor Beam prefab
    public Texture2D crosshairTexture; // Crosshair texture
    public float tractorBeamRange = 10f; // Effective range of the tractor beam
    public float tractorSpeed = 5f; // Speed at which the tractor beam pulls objects

    private GameObject tractorBeamInstance; // Instance of the tractor beam
    private bool isTractorBeamActive = false; // Flag to check if the tractor beam is active

    void Update()
    {
        // Toggle Tractor Beam with the left mouse button (LMB)
        if (Input.GetMouseButtonDown(0))
        {
            ToggleTractorBeam();
        }

        // Rotate the tractor beam along with the ship
        if (isTractorBeamActive)
        {
            tractorBeamInstance.transform.rotation = transform.rotation;
        }

        // Check for left mouse button input to pull objects
        if (Input.GetMouseButton(0) && isTractorBeamActive)
        {
            PullObject();
        }

        // Check for right mouse button input to push objects
        if (Input.GetMouseButtonDown(1) && isTractorBeamActive)
        {
            PushObjects();
        }
    }

    void ToggleTractorBeam()
    {
        Debug.Log("Toggle Tractor Beam");
        if (isTractorBeamActive)
        {
            // Deactivate Tractor Beam
            Destroy(tractorBeamInstance);
            isTractorBeamActive = false;
        }
        else
        {
            // Activate Tractor Beam
            tractorBeamInstance = Instantiate(tractorBeamPrefab, transform.position - transform.up, Quaternion.identity);
            isTractorBeamActive = true;
        }
    }

    void PullObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, tractorBeamRange) && hit.collider.CompareTag("ResourceNode"))
        {
            // Pull the object towards the ship
            hit.rigidbody.AddForce((transform.position - hit.transform.position).normalized * tractorSpeed);
        }
    }

    void PushObjects()
    {
        // Create a force cone in front of the tractor beam
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, tractorBeamRange);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("ResourceNode"))
            {
                // Push objects within the cone away from the ship
                Vector3 forceDirection = (collider.transform.position - transform.position).normalized;
                collider.GetComponent<Rigidbody>().AddForce(forceDirection * tractorSpeed, ForceMode.Impulse);
            }
        }
    }

    void OnGUI()
    {
        // Display crosshair in the center of the screen
        if (isTractorBeamActive)
        {
            float size = 20f;
            GUI.DrawTexture(new Rect((Screen.width - size) / 2, (Screen.height - size) / 2, size, size), crosshairTexture);
        }
    }
}
