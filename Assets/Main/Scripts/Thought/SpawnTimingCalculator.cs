using UnityEngine;

public class SpawnTimingCalculator : ISpawnTimingCalculator
{
    private readonly float baseInterval = 30f;
    private readonly float minInterval = 3f;
    private readonly float decayRate = 0.045f;

    public float CalculateInterval(int mindLevel)
    {
        float interval = minInterval + (baseInterval - minInterval) * Mathf.Exp(-decayRate * (mindLevel + 1));
        float noise = UnityEngine.Random.Range(-0.15f, 0.15f);
        return interval * (1f + noise);
    }
}
