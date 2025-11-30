using System.Collections.Generic;
using UnityEngine;

public class SphereArcBuilder : ISphereArcBuilder
{
    public List<Vector3> SampleArcPoints(ThoughtSpawnPointData data)
    {
        var points = new List<Vector3>();
        Vector3 p0 = data.PointA;
        Vector3 p2 = data.PointB;
        Vector3 mid = (p0 + p2) * 0.5f + Vector3.up * data.ArcHeight;

        Vector3 last = Bezier(p0, mid, p2, 0f);
        points.Add(last);

        for (int i = 1; i <= 100 && points.Count < data.MaxSphereCount; i++)
        {
            float t = i / 100f;
            Vector3 current = Bezier(p0, mid, p2, t);
            if (Vector3.Distance(last, current) >= data.MinDistanceBetweenSpheres)
            {
                points.Add(current);
                last = current;
            }
        }

        return points;
    }

    private Vector3 Bezier(Vector3 a, Vector3 c, Vector3 b, float t)
    {
        Vector3 ab = Vector3.Lerp(a, c, t);
        Vector3 cb = Vector3.Lerp(c, b, t);
        return Vector3.Lerp(ab, cb, t);
    }
}
