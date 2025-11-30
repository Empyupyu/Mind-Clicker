using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public interface IAnalytics
{
    UniTask Initialize();
    public void Send(string eventName);
    public void Send(string eventName, Dictionary<string, object> eventData);
}
