using Cysharp.Threading.Tasks;

public class SaveLoadService
{
    private readonly IPlayerDataProvider provider;

    public SaveLoadService(IPlayerDataProvider provider)
    {
        this.provider = provider;
    }

    public UniTask<PlayerData> Load(string key) => provider.Load(key);
    public UniTask Save(string key, PlayerData data) => provider.Save(key, data);
}
