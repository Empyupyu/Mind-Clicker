public class AuthorizationService
{
    private readonly IAuthorizationProvider authorizationProvider;

    public AuthorizationService(IAuthorizationProvider authorizationProvider)
    {
        this.authorizationProvider = authorizationProvider;
    }

    public void Authorization()
    {
        authorizationProvider.Authorization();
    }

    public bool HasAuthorization()
    {
        return authorizationProvider.HasAuthorization();
    }
}
