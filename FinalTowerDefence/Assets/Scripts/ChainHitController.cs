using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ChainHitController : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public float age = 0;
    public float timeToLive = .5f;

    private void getLineRenderer()
    {
        if (lineRenderer != null) return;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
    }

    void Update()
    {
        age += Time.deltaTime;
        if (age > timeToLive)
        {
            Destroy(gameObject);
            return;
        }

        lineRenderer.startColor = new Color(1, 0, 0, 1 - (age / timeToLive));
        lineRenderer.endColor = new Color(1, 1, 0, .5f * (1 - (age / timeToLive)));
    }

    public void CreateChainHit(IEnumerable<EnemyController> targets)
    {
        getLineRenderer();
        List<Vector3> positions = new List<Vector3>();
        foreach (var enemy in targets)
            positions.Add(enemy.transform.position);
        lineRenderer.numPositions = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
