using System.Collections.Generic;
using System.Linq;

public class SpawnPointSelector : ISpawnPointSelector
{
    private readonly List<ThoughtSpawnPointData> thoughtSpawnPointDatas;
    private readonly List<SpawnPoint> original;
    private readonly List<SpawnPoint> shuffled;

    public SpawnPointSelector(List<ThoughtSpawnPointData> pointDatas)
    {
        original = new List<SpawnPoint>();
        shuffled = new List<SpawnPoint>();

        thoughtSpawnPointDatas = pointDatas;

        for (int i = 0; i < thoughtSpawnPointDatas.Count; i++)
        {
            SpawnPoint sphereArcSpawner = new SpawnPoint(thoughtSpawnPointDatas[i]);
            original.Add(sphereArcSpawner);
            shuffled.Add(sphereArcSpawner);
        }
    }

    public SpawnPoint Select(SpawnPointDirection direction)
    {
        if (direction == SpawnPointDirection.Random)
        {
            shuffled.Shuffle();
            return shuffled.FirstOrDefault(p => !p.IsActive);
        }

        return original[(int)direction];
    }
}
