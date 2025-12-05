using Cysharp.Threading.Tasks;

public class SaveLoadService : IGameModule
{
    public int Priority { get; }

    private readonly IPlayerDataProvider provider;
    private readonly PlayerDataRef playerData;

    public SaveLoadService(
        IPlayerDataProvider provider,
        PlayerDataRef playerData,
        int priority)
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

    public UniTask<PlayerData> Load() => provider.Load(GameConstants.SaveKey);
    public UniTask Save(PlayerData data) => provider.Save(GameConstants.SaveKey, data);
}
