using UnityEngine;

public class ThoughtFormSelector : IThoughtFormSelector
{
    private readonly NegativeThoughtConfig config;

    public ThoughtFormSelector(NegativeThoughtConfig config)
    {
        this.config = config;
    }

    public NegativeThoughtForm SelectRandom(int mindLevel)
    {
        var level = config.NegativeThoughtLevels[Mathf.Clamp(mindLevel, 0, config.NegativeThoughtLevels.Count - 1)];
        return level.NegativeThoughtForms[Random.Range(0, level.NegativeThoughtForms.Count)];
    }
}
