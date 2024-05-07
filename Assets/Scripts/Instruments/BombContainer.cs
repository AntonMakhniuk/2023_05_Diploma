using Assets.Scripts.Instruments;
using Cinemachine;
using UnityEngine;

public class BombContainer : Instrument
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Transform muzzlePoint;
    public float bombSpeed = 5f;
    [SerializeField] private float bombLifetime = 3f;
    [SerializeField] private float bombRange = 5f;
    [SerializeField] private CinemachineVirtualCamera cinematicCamera;
    [SerializeField] private Canvas crosshairCanvas;

    private void Start()
    {
        ToggleInstrument(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ToggleInstrument(isActiveTool);
        }        
    }

    private void FixedUpdate()
    {
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
        GameObject bomb = Instantiate(bombPrefab, muzzlePoint.position, muzzlePoint.rotation); // Adjust position and rotation
        
        Bomb bombScript = bomb.GetComponent<Bomb>();
        if (bombScript != null)
        {
            bombScript.SetBombContainer(this);
        }
        
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        rb.velocity = muzzlePoint.forward * bombSpeed;
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

    void ToggleInstrument(bool activate)
    {
        isActiveTool = activate;
        
        if (isActiveTool)
        {
            cinematicCamera.gameObject.SetActive(true);
            crosshairCanvas.gameObject.SetActive(true);
            SetActiveTool(true);
        }
        else
        {
            cinematicCamera.gameObject.SetActive(false);
            crosshairCanvas.gameObject.SetActive(false);
            SetActiveTool(false);
        }
    }
}
