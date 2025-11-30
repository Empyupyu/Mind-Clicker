using UnityEngine;

public class ThoughtStandartHealthProvider : IThoughtHealthProvider
{
    private readonly NegativeThoughtConfig thoughtConfigs;

    public ThoughtStandartHealthProvider(NegativeThoughtConfig configs)
    {
        thoughtConfigs = configs;
    }

    public float CalculateHealth(NegativeThoughtForm config, int mindLevel)
    {
        if (config.Health > 0)
            return config.Health;

        float effectiveRate = thoughtConfigs.GrowthHPRate + thoughtConfigs.HPAcceleration * mindLevel;
        return thoughtConfigs.BaseHP * Mathf.Pow(1f + effectiveRate, mindLevel);
    }
}
