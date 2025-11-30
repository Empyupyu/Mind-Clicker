using System.Collections.Generic;
using System.Linq;

public class BossEnvironmentRegistry
{
    private readonly Dictionary<ThoughtType, string> references;

    public BossEnvironmentRegistry(BossEnvironmentManifest manifest)
    {
        references = manifest.Environments
            .Where(e => e.Key != "")
            .ToDictionary(e => e.BossType, e => e.Key);
    }

    public string GetReference(ThoughtType bossType)
    {
        return references.TryGetValue(bossType, out var reference) ? reference : null;
    }
}
