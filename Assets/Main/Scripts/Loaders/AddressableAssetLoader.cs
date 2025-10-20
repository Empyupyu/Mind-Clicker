using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class AddressableAssetLoader : IAssetLoader
{
    private readonly Dictionary<string, AsyncOperationHandle> _handles = new();

    public async UniTask<GameObject> LoadPrefab(string key)
    {
        if (_handles.ContainsKey(key))
            return _handles[key].Result as GameObject;

        var handle = Addressables.InstantiateAsync(key);
        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"Failed to load prefab: {key}");
            return null;
        }

        _handles[key] = handle;
        return handle.Result;
    }

    public async UniTask<T> LoadAsset<T>(string key) where T : class
    {
        if (_handles.ContainsKey(key))
            return _handles[key].Result as T;

        var handle = Addressables.LoadAssetAsync<T>(key);
        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"Failed to load asset: {key}");
            return null;
        }

        _handles[key] = handle;
        return handle.Result;
    }

    public async UniTask Unload(string key)
    {
        if (!_handles.ContainsKey(key)) return;

        var handle = _handles[key];
        if (handle.IsValid())
        {
            if (handle.Result is GameObject go)
                Addressables.ReleaseInstance(go);
            else
                Addressables.Release(handle);
        }

        _handles.Remove(key);
        await UniTask.Yield();
    }

    public void UnloadAll()
    {
        foreach (var kvp in _handles)
        {
            var handle = kvp.Value;
            if (handle.IsValid())
            {
                if (handle.Result is GameObject go)
                    Addressables.ReleaseInstance(go);
                else
                    Addressables.Release(handle);
            }
        }

        _handles.Clear();
    }
}
