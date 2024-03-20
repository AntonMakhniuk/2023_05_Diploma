using System;
using System.Collections;
using Production.Crafting;
using Production.Systems;
using UnityEngine;

namespace Assets.Scripts.Player {
public class PlayerProductionManager : MonoBehaviour {
    [SerializeField] private GameObject containerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private InventoryWindow inventory;
    private GameObject container;

    private void Start()
    {
        //ProductionSessionManager.OnProductionFinishedSuccessfully += SpawnContainer;
    }

    public void SpawnContainer() {
        container = Instantiate(containerPrefab, spawnPoint.position, Quaternion.identity);
        container.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-10f,0,0), ForceMode.Impulse);
        StartCoroutine(EnableColliderAfterSeconds());
        inventory.DecreaseFuelGasQuantity(200);
        inventory.DecreaseSpaceOreQuantity(10);
    }
    
    public void SpawnContainer(object sender, CraftingData craftingData) {
        container = Instantiate(containerPrefab, spawnPoint.position, Quaternion.identity);
        container.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-10f,0,0), ForceMode.Impulse);
        StartCoroutine(EnableColliderAfterSeconds());
    }

    private IEnumerator EnableColliderAfterSeconds() {
        yield return new WaitForSeconds(1f);
        container.GetComponent<BoxCollider>().enabled = true;
    }
}
}