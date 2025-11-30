using Cysharp.Threading.Tasks;

public class AnalyticsModule : IGameModule
{
    public int Priority { get; }
    private readonly IAnalytics analytics;

    public AnalyticsModule(IAnalytics analyticsService, int priority)
    {
        analytics = analyticsService;
        Priority = priority;
    }
    public async UniTask InitializeAsync()
    {
        await analytics.Initialize();
    }
}
