using UnityEngine;

public interface ISphereArcAnimator
{
    void Animate(GameObject sphere, int index, int total, Vector3 pos, ThoughtSpawnPointData data);
}
