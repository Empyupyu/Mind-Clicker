using Cysharp.Threading.Tasks;

public class SaveLoadService : IGameModule
{
    public int Priority { get; }

    private readonly IPlayerDataProvider provider;
    private readonly PlayerDataRef playerData;
    private const string SAVE_KEY = "PlayerDataSaveKey";

    public SaveLoadService(IPlayerDataProvider provider, PlayerDataRef playerData, int priority)
    {
        this.provider = provider;
        this.playerData = playerData;
        Priority = priority;
    }

    public async UniTask InitializeAsync()
    {
        var data = await Load();
        playerData.Set(data);
        playerData.Set(new PlayerData());
    }

    public UniTask<PlayerData> Load() => provider.Load(SAVE_KEY);
    public UniTask Save(PlayerData data) => provider.Save(SAVE_KEY, data);
}
