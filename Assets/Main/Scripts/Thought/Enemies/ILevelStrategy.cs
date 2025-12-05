public interface ILevelStrategy
{
    public ThoughtType ThoughtType { get;}
    public void Run(NegativeThoughtForm form);
    public void Exit();
}