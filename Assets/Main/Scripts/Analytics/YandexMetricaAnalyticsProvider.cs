using System.Collections.Generic;
using YG;

public class YandexMetricaAnalyticsProvider : IAnalyticsProvider
{
    public void Send(string eventName)
    {
        YG2.MetricaSend(eventName);
    }

    public void Send(string eventName, Dictionary<string, object> eventData)
    {
        YG2.MetricaSend(eventName, eventData);
    }
}
