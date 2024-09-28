using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Testing
{
    public class CursorMovements : MonoBehaviour
    {
        [SerializeField] private Transform ship;
        [SerializeField] private CinemachineFreeLook freeLookCamera;
        [SerializeField] private float distanceFromShip = 15f;

        void Update()
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            Vector3 targetPosition = ray.GetPoint(distanceFromShip);
            targetPosition.z = Mathf.Clamp(targetPosition.z, ship.position.z - 20, ship.position.z + 20);

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
        }
    }
}
