using System.Collections.Generic;
using System.Linq;

public class SpawnPointSelector : ISpawnPointSelector
{
    private readonly List<SphereArcSpawner> original;
    private readonly List<SphereArcSpawner> shuffled;

    public SpawnPointSelector(List<SphereArcSpawner> spawnPoints)
    {
        original = new List<SphereArcSpawner>(spawnPoints);
        shuffled = new List<SphereArcSpawner>(spawnPoints);
    }

    public SphereArcSpawner Select(SpawnPointDirection direction)
    {
        if (direction == SpawnPointDirection.Random)
        {
            shuffled.Shuffle();
            return shuffled.FirstOrDefault(p => !p.IsActive);
        }

        return original[(int)direction];
    }
}
