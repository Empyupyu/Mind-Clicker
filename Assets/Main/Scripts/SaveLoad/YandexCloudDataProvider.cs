using Cysharp.Threading.Tasks;
using YG;

public class YandexCloudDataProvider : IPlayerDataProvider
{
    private readonly DefaultPlayerDataProvider defaultPlayerDataProvider;

    public YandexCloudDataProvider(DefaultPlayerDataProvider defaultPlayerDataProvider)
    {
        this.defaultPlayerDataProvider = defaultPlayerDataProvider;
    }

    public async UniTask<PlayerData> Load(string key)
    {
        await UniTask.WaitUntil(() => YG2.isSDKEnabled);

        var saves = YG2.saves.PlayerData;
        if (saves == null)
        {
            saves = defaultPlayerDataProvider.CreateDefault();

            YG2.SetDefaultSaves();
            return saves;
        }

        return saves;
    }

    public async UniTask Save(string key, PlayerData data)
    {
        YG2.saves.PlayerData = data;
        YG2.SaveProgress();

        await UniTask.CompletedTask;
    }
}
