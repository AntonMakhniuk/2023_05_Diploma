using UnityEngine;

public class ShipInventory : MonoBehaviour
{
    public GameObject tractorBeamPrefab;
    public GameObject gasCollectorPrefab;
    public Transform shipTransform; 
    

    private TractorBeam tractorBeamInstance;
    private GasCollector gasCollectorInstance;
    
    
    void Update()
    {
        // Toggle Tractor Beam with the key (1)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleTractorBeam();
        }
        // Toggle Gas Collector with the key (3)
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ToggleGasCollector();
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
    void ToggleGasCollector()
    {
        if (gasCollectorInstance == null)
        {
            // Instantiate Gas Collector and set shipTransform as the parent
            gasCollectorInstance = Instantiate(gasCollectorPrefab, shipTransform).GetComponent<GasCollector>();
            gasCollectorInstance.transform.localPosition = Vector3.left * gasCollectorInstance.GasCollectorOffset; // Adjust offset to spawn horizontally
        }
        else
        {
            // Show the gas collected in the Gas Collector before deactivating it
            if (gasCollectorInstance != null)
            {
                float collectedGas = gasCollectorInstance.GetCurrentGasStorage();
                Debug.Log("Collected Gas: " + collectedGas);
            }

            // Deactivate Gas Collector
            Destroy(gasCollectorInstance.gameObject);
            gasCollectorInstance = null;
        }
    }


}