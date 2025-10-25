using Cysharp.Threading.Tasks;

public class Tier1EnemyThoughtLogic : IThoughtLogic
{
    public ThoughtType ThoughtType { get; }

    private readonly ThoughtSpawner thoughtSpawner;

    public Tier1EnemyThoughtLogic(ThoughtType thoughtType, ThoughtSpawner thoughtSpawner) 
    {
        ThoughtType = thoughtType;
        this.thoughtSpawner = thoughtSpawner;
    }

    public void AttachTo(NegativeThought thought) 
    {
        thoughtSpawner.SetDelayNextSpawn().Forget();
    }
}
