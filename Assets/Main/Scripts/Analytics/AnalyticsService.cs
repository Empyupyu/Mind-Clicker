using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class AnalyticsService : IAnalytics
{
    private readonly IAnalyticsProvider analyticsProvider;

    public AnalyticsService(IAnalyticsProvider analyticsProvider)
    {
        this.analyticsProvider = analyticsProvider;
    }

    public UniTask Initialize()
    {
        return UniTask.CompletedTask;
    }

    public void Send(string eventName)
    {
        analyticsProvider.Send(eventName);
    }

    public void Send(string eventName, Dictionary<string, object> eventData)
    {
        analyticsProvider.Send(eventName, eventData);
    }
}
