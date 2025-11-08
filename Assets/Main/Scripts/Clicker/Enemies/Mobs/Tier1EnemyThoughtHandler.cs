using Cysharp.Threading.Tasks;

public class Tier1EnemyThoughtHandler : IThoughtHandler
{
    public ThoughtType ThoughtType { get; }

    private readonly ThoughtSpawner thoughtSpawner;

    public Tier1EnemyThoughtHandler(ThoughtType thoughtType, ThoughtSpawner thoughtSpawner) 
    {
        ThoughtType = thoughtType;
        this.thoughtSpawner = thoughtSpawner;
    }

    public void AttachTo(NegativeThought thought) 
    {
        thoughtSpawner.SpawnWithDelay().Forget();
    }
}
