using System.Collections.Generic;
using UnityEngine;

public interface ISphereArcBuilder
{
    List<Vector3> SampleArcPoints(ThoughtSpawnPointData data);
}
