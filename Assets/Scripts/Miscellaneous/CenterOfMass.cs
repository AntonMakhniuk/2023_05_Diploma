using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CenterOfMass : MonoBehaviour
{
    [SerializeField] private Vector3 centerOfMass;
    protected Rigidbody rd;
    
    void Start()
    {
        rd = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        centerOfMass = rd.centerOfMass;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.rotation * centerOfMass, 1f);
    }
}
