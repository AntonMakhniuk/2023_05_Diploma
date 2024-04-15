using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject teleporterPrefab;
    public GameObject acceleratorPrefab;
    public GameObject refillStationPrefab;

    private GameObject currentObject;

    public void BuildObject(GameObject prefab)
    {
        if (prefab != null)
        {
            Vector3 spawnPosition = transform.position + transform.forward * 5f;
            GameObject newObj = InstantiateAndCustomize(prefab, spawnPosition);
            CurrentObject = newObj;
        }
        else
        {
            Debug.LogError("Prefab is not assigned.");
        }
    }

    public void RemoveCurrentObject()
    {
        if (CurrentObject != null)
        {
            Destroy(CurrentObject);
            CurrentObject = null;
        }
    }

    public void FixateObject()
    {
        if (CurrentObject != null)
        {
            // Detach the object from its parent
            CurrentObject.transform.parent = null;

            // Perform any additional cleanup or finalization logic here
            // For example, you might deactivate specific components or apply final transformations

            // Clear the current object reference
            CurrentObject = null;
        }
    }

    private GameObject InstantiateAndCustomize(GameObject prefab, Vector3 position)
    {
        GameObject newObj = Instantiate(prefab, position, transform.rotation, transform);
        SetObjectOpacity(newObj, 0.3f);
        return newObj;
    }

    private void SetObjectOpacity(GameObject obj, float opacity)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            Material objMaterial = objRenderer.material;
            Color materialColor = objMaterial.color;
            materialColor.a = opacity;
            objMaterial.color = materialColor;

            objMaterial.SetFloat("_Mode", 3);
            objMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            objMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            objMaterial.SetInt("_ZWrite", 0);
            objMaterial.DisableKeyword("_ALPHATEST_ON");
            objMaterial.EnableKeyword("_ALPHABLEND_ON");
            objMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            objMaterial.renderQueue = 3000;
        }
    }

    public GameObject CurrentObject
    {
        get { return currentObject; }
        private set { currentObject = value; }
    }
}
