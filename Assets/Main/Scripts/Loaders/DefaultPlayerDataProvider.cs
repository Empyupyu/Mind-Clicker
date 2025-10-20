public class DefaultPlayerDataProvider
{
    private readonly PlayerDefaultSettings defaultConfig;

    public DefaultPlayerDataProvider(PlayerDefaultSettings config)
    {
        defaultConfig = config;
    }

    public PlayerData CreateDefault()
    {
        return new PlayerData
        {
            CurrentLevel = defaultConfig.CurrentLevel,
            
            Upgrades = new System.Collections.Generic.List<UpgradeProgress>(),
        };
    }
}
