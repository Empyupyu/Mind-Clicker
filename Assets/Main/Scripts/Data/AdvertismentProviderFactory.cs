using System;

public static class AdvertismentProviderFactory
{
    public static Type GetProviderType(PlayerDataProviderType type)
    {
        return type switch
        {
            PlayerDataProviderType.Yandex => typeof(YandexAdvertisementProvider),
            PlayerDataProviderType.Local => null,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
