using System;
using Zenject;

public class GameData
{
    public ThoughtSpawner ThoughtSpawner;
    public GameObjectContext Level;

    public float DamagePerClick = 1;
    public float DamagePerSecond;
}

public static class PlayerDataProviderFactory
{
    public static Type GetProviderType(PlayerDataProviderType type)
    {
        return type switch
        {
            PlayerDataProviderType.Yandex => typeof(YandexCloudDataProvider),
            PlayerDataProviderType.Local => typeof(LocalJsonDataProvider),
            //PlayerDataProviderType.Steam => typeof(SteamDataProvider),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
