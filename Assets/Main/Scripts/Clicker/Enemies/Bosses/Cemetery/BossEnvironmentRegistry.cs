using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;

public class BossEnvironmentRegistry
{
    private readonly Dictionary<ThoughtType, AssetReferenceGameObject> references;

    public BossEnvironmentRegistry(BossEnvironmentManifest manifest)
    {
        references = manifest.Environments
            .Where(e => e.EnvironmentPrefab != null)
            .ToDictionary(e => e.BossType, e => e.EnvironmentPrefab);
    }

    public AssetReferenceGameObject GetReference(ThoughtType bossType)
    {
        return references.TryGetValue(bossType, out var reference) ? reference : null;
    }
}
