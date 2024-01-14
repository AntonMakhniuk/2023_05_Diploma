using UnityEngine;

public class TractorBeamSpawner : MonoBehaviour
{
    public GameObject tractorBeamPrefab;
    public Transform shipTransform; // Assign the ship's transform in the Inspector

    private TractorBeam tractorBeamInstance;
    
    void Update()
    {
        // Toggle Tractor Beam with the key (1)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleTractorBeam();
        }
    }

    void ToggleTractorBeam()
    {
        if (tractorBeamInstance == null)
        {
            // Instantiate Tractor Beam and set shipTransform as the parent
            tractorBeamInstance = Instantiate(tractorBeamPrefab, shipTransform).GetComponent<TractorBeam>();
            tractorBeamInstance.transform.localPosition = Vector3.right * tractorBeamInstance.TractorBeamOffset; // Adjust offset to spawn horizontally
            tractorBeamInstance.ActivateTractorBeam(); // Activate the tractor beam
        }
        else
        {
            // Deactivate Tractor Beam
            tractorBeamInstance.DeactivateTractorBeam();
            Destroy(tractorBeamInstance.gameObject);
            tractorBeamInstance = null;
        }
    }
}