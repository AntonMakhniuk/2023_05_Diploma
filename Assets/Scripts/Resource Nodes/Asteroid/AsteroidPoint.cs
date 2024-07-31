using System;
using Assets.Scripts.Instruments;
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
