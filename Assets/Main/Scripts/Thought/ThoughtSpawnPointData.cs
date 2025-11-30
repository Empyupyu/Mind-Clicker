using UnityEngine;

[CreateAssetMenu(fileName = "ThoughtSpawnPointData", menuName = "Create Game Datas/ThoughtSpawnPointData")]
public class ThoughtSpawnPointData : ScriptableObject
{
    [field: SerializeField] public Vector3 PointA { get; private set; }
    [field: SerializeField] public Vector3 PointB { get; private set; }
    [field: SerializeField] public float ArcHeight { get; private set; } = 3f;
    [field: SerializeField] public float MinDistanceBetweenSpheres { get; private set; } = 1f;
    [field: SerializeField] public int MaxSphereCount { get; private set; } = 10;
    [field: SerializeField] public Vector3 TargetOffset { get; private set; }
    [field: SerializeField] public float BaseScale { get; private set; } = 0.5f;
    [field: SerializeField] public float ScaleExponent { get; private set; } = 2f;
    [field: SerializeField] public float BaseDuration { get; private set; } = 1.2f;
    [field: SerializeField] public float DurationDecay { get; private set; } = 0.1f;
    [field: SerializeField] public float DelayStep { get; private set; } = 0.1f;
    [field: SerializeField] public float ApproachDistance { get; private set; } = 2f;
    [field: SerializeField] public float ScaleMinDuration { get; private set; } = 0.2f;
    [field: SerializeField] public float MoveAmplitudeMin { get; private set; } = 0.05f;
    [field: SerializeField] public float MoveAmplitudeMax { get; private set; } = 0.15f;
    [field: SerializeField] public float MoveDurationMin { get; private set; } = 1.5f;
    [field: SerializeField] public float MoveDurationMax { get; private set; } = 3f;
}