using Cysharp.Threading.Tasks;
using YG;

public interface IGameModule
{
    int Priority { get; }
    UniTask InitializeAsync();
}

public interface ILeaderboard
{

}


public class YandexLeaderboard : ILeaderboard
{
    public void SetNewRecord()
    {
        YG2.SetLeaderboard("TechnoNameLB", 100);
    }
}