using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Skybox))] 
public class SkyboxSetter : MonoBehaviour
{

    [SerializeField] List<Material> _skyboxMaterials;

    Skybox _skybox;

    private void Awake()
    {
        _skybox = GetComponent<Skybox>();
    }

    private void OnEnable()
    {
        ChangeSkybox(0);
    }

    void ChangeSkybox(int skybox)
    {
        if (_skybox != null && skybox >= 0 && skybox <= _skyboxMaterials.Count)
        {
            _skybox.material = _skyboxMaterials[skybox];
        }
    }
}
