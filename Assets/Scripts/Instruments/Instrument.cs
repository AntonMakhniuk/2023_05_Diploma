﻿using UnityEngine;

namespace Assets.Scripts.Instruments {
[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class Instrument : MonoBehaviour {
    protected bool isActiveTool = false;
    private Renderer meshRenderer;
    private Collider instrumentCollider;

    protected virtual void Awake()
    {
        // Get the renderer and collider components
        meshRenderer = GetComponent<Renderer>();
        instrumentCollider = GetComponent<Collider>();

        // Hide the instrument by default
        meshRenderer.enabled = false;
        instrumentCollider.enabled = false;
    }

    public virtual void Toggle() {
        meshRenderer.enabled = !meshRenderer.enabled;
        instrumentCollider.enabled = !instrumentCollider.enabled;
    }
    
    // Set the instrument as the active tool
    public void SetActiveTool(bool active)
    {
        isActiveTool = active;
    }
}
}