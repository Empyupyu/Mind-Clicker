public interface IEnvironmentProvider
{
   public string GetDeviceType();
   public bool IsDesktop();
   public bool IsMobile();
   public bool IsTablet();
   public bool IsTV();
   public string GetLanguage();
   public string GetPlatform();
}
