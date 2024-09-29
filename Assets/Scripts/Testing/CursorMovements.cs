using UnityEngine;

public class CursorMovements : MonoBehaviour
{
    [SerializeField] private Transform ship;
    [SerializeField] private float distanceFromShip = 15f;
    [SerializeField] private float followSmoothing = 0.1f;

    private Vector3 currentVelocity = Vector3.zero;

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        Vector3 targetPosition = ray.GetPoint(distanceFromShip);


        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, followSmoothing);
        transform.position = smoothPosition;
    }
}
