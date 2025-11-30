using YG;

public class YandexAuthorizationProvider : IAuthorizationProvider
{
    public void Authorization()
    {
        YG2.OpenAuthDialog();
    }

    public bool HasAuthorization()
    {
        return YG2.player.auth;
    }
}
