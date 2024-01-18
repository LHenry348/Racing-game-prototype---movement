using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireTrackEffectHandler : MonoBehaviour
{
    CarController carController;
    TrailRenderer trailRenderer;

    void Awake()
    {
        carController = GetComponentInParent<CarController>();

        trailRenderer = GetComponent<TrailRenderer>();

        trailRenderer.emitting = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (carController.IsDrifting(out float lateralVelocity, out bool isBraking))
        {
            trailRenderer.emitting = true;
        }

        else
        {
            trailRenderer.emitting = false;
        }
    }
}
