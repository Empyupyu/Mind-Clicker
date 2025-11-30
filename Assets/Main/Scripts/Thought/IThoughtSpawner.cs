using Cysharp.Threading.Tasks;

public interface IThoughtSpawner
{
    void Spawn();
    UniTask SpawnWithDelay();
    void DestroyAll();
    NegativeThought GetTarget();
    ThoughtUIView GetRandomView();
}