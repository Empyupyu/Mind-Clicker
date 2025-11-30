using Cysharp.Threading.Tasks;

public interface IPlayerDataProvider
{
    UniTask<PlayerData> Load(string key);
    UniTask Save(string key, PlayerData data);
}
