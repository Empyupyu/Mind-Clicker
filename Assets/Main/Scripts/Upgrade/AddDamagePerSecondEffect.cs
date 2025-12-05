public class AddDamagePerSecondEffect : IUpgradeEffect
{
    public UpgradeType Type { get; private set; }

    private readonly GameData gameData;

    public AddDamagePerSecondEffect(GameData gameData, UpgradeType type)
    {
        this.gameData = gameData;
        Type = type;
    }

    public void Apply(UpgradeConfig config, int level)
    {
        if(level > 1)
        {
            gameData.DamagePerSecond -= config.BaseEffect + (config.BaseEffect * (config.EffectMultiplier * (level - 1)));
        }

        gameData.DamagePerSecond += config.BaseEffect + (config.BaseEffect * (config.EffectMultiplier * level));
    }
}