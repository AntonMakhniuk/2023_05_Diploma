using UnityEngine;

public class GasCloudScript : MonoBehaviour
{
    [SerializeField]private float initialGasCapacity = 100f;
    private float gasCapacity;

    void Start()
    {
        gasCapacity = initialGasCapacity;
    }

    public void DecreaseGasCapacity(float amount)
    {
        gasCapacity -= amount;
    }

    public float GetGasCapacity()
    {
        return gasCapacity;
    }
}