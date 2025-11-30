public interface IThoughtViewPool
{
    int GetPoolCount();
    ThoughtUIView Get();
    void Register(ThoughtUIView view);
    void Release(NegativeThought thought);
    ThoughtUIView GetRandom();
}
