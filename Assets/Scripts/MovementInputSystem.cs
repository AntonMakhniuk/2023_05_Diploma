using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInputSystem : MonoBehaviour
{
    private Rigidbody ship;

    private void Awake()
    {
        ship = GetComponent<Rigidbody>();
          
    }
    public void Forward()
    {
        Debug.Log("W");
    }
}
