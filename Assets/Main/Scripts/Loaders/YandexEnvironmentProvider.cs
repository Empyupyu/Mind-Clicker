using YG;

public class YandexEnvironmentProvider : IEnvironmentProvider
{
    public string GetDeviceType() => YG2.envir.deviceType;
    public bool IsDesktop() => YG2.envir.isDesktop;
    public bool IsMobile() => YG2.envir.isMobile;
    public bool IsTablet() => YG2.envir.isTablet;
    public bool IsTV() => YG2.envir.isTV;
    public string GetLanguage() => YG2.envir.language;
    public string GetPlatform() => YG2.envir.platform;
}
