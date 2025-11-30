using System.Collections.Generic;

public interface IAnalyticsProvider
{
    public void Send(string eventName);
    public void Send(string eventName, Dictionary<string, object> eventData);
}
