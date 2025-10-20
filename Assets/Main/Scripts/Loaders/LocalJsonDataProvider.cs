using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine;

public class LocalJsonDataProvider : IPlayerDataProvider
{
    private readonly string rootPath;
    private readonly DefaultPlayerDataProvider defaultPlayerDataProvider;

    public LocalJsonDataProvider(DefaultPlayerDataProvider defaultPlayerDataProvider, string rootPath = null)
    {
        this.rootPath = rootPath ?? Application.persistentDataPath;
        this.defaultPlayerDataProvider = defaultPlayerDataProvider;
    }

    public async UniTask<PlayerData> Load(string key)
    {
        string path = Path.Combine(rootPath, key + ".json");
        if (!File.Exists(path))
        {
            var newData = defaultPlayerDataProvider.CreateDefault();
            return newData;

        }
        string json = await File.ReadAllTextAsync(path);
        return JsonUtility.FromJson<PlayerData>(json);
    }

    public async UniTask Save(string key, PlayerData data)
    {
        string path = Path.Combine(rootPath, key + ".json");
        string json = JsonUtility.ToJson(data, true);
        await File.WriteAllTextAsync(path, json);
    }
}
