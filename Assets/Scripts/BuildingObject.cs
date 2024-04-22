using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class BuildingObject : MonoBehaviour
{
    private static Dictionary<Type, List<BuildingObject>> instancesByType = new Dictionary<Type, List<BuildingObject>>();

    protected virtual void Awake()
    {
        AddInstanceToDictionary(this);

    }

    protected virtual void OnDestroy()
    {
        RemoveInstanceFromDictionary(this);
    }


    private static void AddInstanceToDictionary(BuildingObject instance)
    {
        Type type = instance.GetType();
        if (!instancesByType.ContainsKey(type))
        {
            instancesByType[type] = new List<BuildingObject>();
        }
        instancesByType[type].Add(instance);
    }

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

    public static List<T> GetInstancesOfType<T>() where T : BuildingObject
    {
        Type type = typeof(T);
        if (instancesByType.ContainsKey(type))
        {
            return instancesByType[type].ConvertAll(obj => (T)obj);
        }
        return null;
    }
}
