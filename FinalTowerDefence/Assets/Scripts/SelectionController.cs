using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(LineRenderer))]
public class SelectionController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public LineRenderer lineRenderer;

    private float range = 0;
    public float Range
    {
        get
        {
            return range;
        }
        set
        {
            if (value < 0) value = 0;
            if (range == value) return;
            range = value;
            if (range == 0)
            {
                spriteRenderer.enabled = true;
                lineRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = false;
                lineRenderer.enabled = true;
                makeLineRendererPositions();
            }
        }
    }
    private void makeLineRendererPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        const int max_segments = 32;
        //var center = new Vector2(transform.localPosition.x + 30, transform.localPosition.y + 30);
        var center = new Vector2(30, 30);
        for (int q = 0; q < max_segments; q++)
        {
            var anglefrom = ((Mathf.PI * 2) / max_segments) * q;
            Vector3 frompos = new Vector3(center.x + Mathf.Sin(anglefrom) * range * 60, center.y + Mathf.Cos(anglefrom) * range * 60, 10);
            positions.Add(frompos);
        }
        positions.Add(positions[0]);

        lineRenderer.numPositions = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    private void Start()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();

        spriteRenderer.enabled = true;
        lineRenderer.enabled = true;
    }
}
