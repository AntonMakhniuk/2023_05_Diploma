using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AsteroidPoint : MonoBehaviour
{
    private Asteroid parentAsteroid;

    private void Start()
    {
        parentAsteroid = GetComponentInParent<Asteroid>();
    }
}
