using Cysharp.Threading.Tasks;

public class SaveLoadService
{
    private readonly IPlayerDataProvider provider;
    private const string SAVE_KEY = "SavePlayerDataKey";

    public SaveLoadService(IPlayerDataProvider provider)
    {
        this.provider = provider;
    }

    public UniTask<PlayerData> Load(string key) => provider.Load(key);
    public UniTask Save(PlayerData data) => provider.Save(SAVE_KEY, data);
}
