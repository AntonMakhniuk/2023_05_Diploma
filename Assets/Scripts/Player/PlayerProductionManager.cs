using System;
using System.Collections;
using Production.Crafting;
using UnityEngine;

namespace Assets.Scripts.Player {
public class PlayerProductionManager : MonoBehaviour {
    [SerializeField] private GameObject containerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private InventoryWindow inventory;
    private GameObject container;

    public void SpawnContainer() {
        container = Instantiate(containerPrefab, spawnPoint.position, Quaternion.identity);
        container.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-10f,0,0), ForceMode.Impulse);
        StartCoroutine(EnableColliderAfterSeconds());
        inventory.DecreaseFuelGasQuantity(200);
        inventory.DecreaseSpaceOreQuantity(10);
    }

    private IEnumerator EnableColliderAfterSeconds() {
        yield return new WaitForSeconds(1f);
        container.GetComponent<BoxCollider>().enabled = true;
    }
}
}