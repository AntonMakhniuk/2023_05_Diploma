using Assets.Scripts.Instruments;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombContainer : Instrument
{
    public GameObject bombPrefab;
    public float bombSpeed = 5f;
    public float bombLifetime = 3f;
    public float bombRange = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ToggleActiveState();
        }
        if (isActiveTool && Input.GetMouseButtonDown(0))
        {
            SpawnBomb();
        }

        if (isActiveTool && Input.GetMouseButtonDown(1))
        {
            DetonateAllBombs();
        }
    }

    void OnDrawGizmos()
    {
        if (isActiveTool)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, bombRange);
        }
    }

    void SpawnBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        
        // Set reference to the BombContainer script on the instantiated bomb
        Bomb bombScript = bomb.GetComponent<Bomb>();
        if (bombScript != null)
        {
            bombScript.SetBombContainer(this);
        }
        
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bombSpeed;
        Destroy(bomb, bombLifetime);
    }

    void DetonateAllBombs()
    {
        Bomb[] bombs = FindObjectsOfType<Bomb>();
        foreach (Bomb bomb in bombs)
        {
            bomb.Detonate();
        }
    }

    void ToggleActiveState()
    {
        SetActiveTool(!isActiveTool);
    }
}
