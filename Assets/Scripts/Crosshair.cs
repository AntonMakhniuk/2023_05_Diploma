using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{ 
    public Image crosshairImage;
    public float maxDistance = 100f;

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.CompareTag("AsteroidPoint"))
            {
                crosshairImage.color=Color.green;
            }
        }
        
        else
        {
            crosshairImage.color=Color.white;
        }
    }
}
