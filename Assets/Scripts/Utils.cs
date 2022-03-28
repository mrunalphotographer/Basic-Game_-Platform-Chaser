using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class GameObjectEfx
{
    public static void DrawCircle(this GameObject container, float radius, float lineWidth)
    {
        var lineRenderer = container.AddComponent<LineRenderer>();
        var segment = 360;

        lineRenderer.positionCount = segment + 1;
        var points = new Vector3[lineRenderer.positionCount];

        for (int i = 0; i < points.Length; i++)
        {
            var rad = Mathf.Deg2Rad * i;
            points[i] = new Vector3(Mathf.Cos(rad) * radius, 0, Mathf.Sin(rad) * radius);
        }

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lineRenderer.useWorldSpace = false;
        lineRenderer.SetPositions(points);


    }
}
