using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class AuthorizationModule : IGameModule
{
    public int Priority { get; }

    private readonly AuthorizationService authorizationService;

    public AuthorizationModule(AuthorizationService authorizationService, int priority)
    {
        this.authorizationService = authorizationService;
        Priority = priority;
    }

    public async UniTask InitializeAsync()
    {
        authorizationService.Authorization();

        try
        {
            await UniTask.WaitUntil(() => authorizationService.HasAuthorization())
                .Timeout(TimeSpan.FromSeconds(GameConstants.AuthorizationTimeout));
        }
        catch (TimeoutException)
        {
            Debug.LogWarning("Authorization timed out. Switching to guest mode.");
        }
    }
}
