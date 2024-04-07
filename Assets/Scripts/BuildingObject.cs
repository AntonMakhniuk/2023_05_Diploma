using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class BuildingObject : MonoBehaviour
{
    protected GameObject currentObject; // Reference to the currently spawned object

    // Static dictionary to store instances based on class type
    private static Dictionary<Type, List<BuildingObject>> instancesByType = new Dictionary<Type, List<BuildingObject>>();

    protected virtual void Awake()
    {
        // Add this instance to the dictionary based on its type
        AddInstanceToDictionary(this);

        foreach (var kvp in instancesByType)
        {
            Type type = kvp.Key;
            List<BuildingObject> instances = kvp.Value;
            Debug.Log($"Type: {type}, Count: {instances.Count}");

            // Optionally, you can log each instance in the list
            foreach (BuildingObject instance in instances)
            {
                Debug.Log($" - {instance.gameObject.name}");
            }
        }
    }

    protected virtual void OnDestroy()
    {
        // Remove this instance from the dictionary based on its type
        RemoveInstanceFromDictionary(this);
    }

    // Method to set the opacity of an object
    public void SetObjectOpacity(float opacity)
    {
        Renderer objRenderer = GetComponent<Renderer>();
        if (objRenderer != null)
        {
            Material objMaterial = objRenderer.material;
            Color materialColor = objMaterial.color;
            materialColor.a = opacity;
            objMaterial.color = materialColor;

            // Configure material for transparency
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

    // Method to add an instance to the dictionary based on its type
    private static void AddInstanceToDictionary(BuildingObject instance)
    {
        Type type = instance.GetType();
        if (!instancesByType.ContainsKey(type))
        {
            instancesByType[type] = new List<BuildingObject>();
        }
        instancesByType[type].Add(instance);
    }

    // Method to remove an instance from the dictionary based on its type
    private static void RemoveInstanceFromDictionary(BuildingObject instance)
    {
        Type type = instance.GetType();
        if (instancesByType.ContainsKey(type))
        {
            instancesByType[type].Remove(instance);
            if (instancesByType[type].Count == 0)
            {
                instancesByType.Remove(type);
            }
        }
    }

    // Method to get all instances of a specific type from the dictionary
    public static List<T> GetInstancesOfType<T>() where T : BuildingObject
    {
        Type type = typeof(T);
        if (instancesByType.ContainsKey(type))
        {
            return instancesByType[type].ConvertAll(obj => (T)obj);
        }
        return new List<T>();
    }
}
