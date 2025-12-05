using Cysharp.Threading.Tasks;

public interface IThoughtSpawner
{
    void Spawn(NegativeThoughtForm form);
    void DestroyAll();
    NegativeThought GetTarget();
    ThoughtUIView GetRandomView();
}