using System;

public static class PlayerDataProviderFactory
{
    public static Type GetProviderType(PlayerDataProviderType type)
    {
        return type switch
        {
            PlayerDataProviderType.Yandex => typeof(YandexCloudDataProvider),
            PlayerDataProviderType.Local => typeof(LocalJsonDataProvider),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
