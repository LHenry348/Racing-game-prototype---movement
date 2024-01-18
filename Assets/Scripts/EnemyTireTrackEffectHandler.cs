using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTireTrackEffectHandler : MonoBehaviour
{
    EnemyController enemyController;
    TrailRenderer trailRenderer;

    void Awake()
    {
        enemyController = GetComponentInParent<EnemyController>();

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
        if (enemyController.IsDrifting(out float lateralVelocity, out bool isBraking))
        {
            trailRenderer.emitting = true;
        }

        else
        {
            trailRenderer.emitting = false;
        }
    }
}
