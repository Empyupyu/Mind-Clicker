public interface IThoughtViewPool
{
    ThoughtUIView Get();
    void Register(ThoughtUIView view);
    void Release(NegativeThought thought);
    ThoughtUIView GetRandom();
}
