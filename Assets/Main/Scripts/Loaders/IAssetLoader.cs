using UnityEngine;
using Cysharp.Threading.Tasks;

public interface IAssetLoader
{
    UniTask<GameObject> LoadPrefab(string key);
    UniTask<T> LoadAsset<T>(string key) where T : class;
    UniTask Unload(string key);
    void UnloadAll();
}
