using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMatcher : MonoBehaviour
{
    private Transform _target;

    private void Awake()
    {
        _target = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.rotation = _target.rotation;
    }
}
