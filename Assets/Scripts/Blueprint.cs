using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour {
    [SerializeField] private int containerCounter = 0;
    public GameObject buildingPrefab;
    [SerializeField] private float pullSpeed = 10f;

    [SerializeField] private List<GameObject> containers = new List<GameObject>();
    [SerializeField] private Rigidbody[] rigidbodies = new Rigidbody[3];
    
    private void Awake() {
        throw new NotImplementedException();
    }

    private void Update() {
        if (containerCounter <= 3 && (containers.Count <= 3 && containers.Count > 0)) {
            PullContainers();
        } else if (containerCounter == 3) {
            TurnToBuilding();
            containerCounter += 1;
        }
    }

    private void TurnToBuilding() {
        Instantiate(buildingPrefab, transform.position, Quaternion.identity);
        buildingPrefab.GetComponent<SphereCollider>().enabled = false;
        buildingPrefab.layer = 11;
        Destroy(transform.gameObject);
    }
    
    private void PullContainers() {
        foreach (var rigidbody in rigidbodies) {
            if (rigidbody != null) 
                rigidbody.AddForce(pullSpeed * (transform.position - rigidbody.gameObject.transform.position).normalized);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Container")) {
            rigidbodies[containers.FindIndex(c => c.gameObject == other.gameObject)] = null;
            containers.Remove(other.gameObject);
            containerCounter += 1;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Container") && containers.Count <= 3) {
            containers.Add(other.gameObject);
            rigidbodies[containers.Count - 1] = other.gameObject.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Container")) {
            rigidbodies[containers.FindIndex(c => c.gameObject == other.gameObject)] = null;
            containers.Remove(other.gameObject);
        }
        
    }
}
