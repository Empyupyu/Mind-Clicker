public interface IThoughtLogic
{
    public ThoughtType ThoughtType { get;}
    public void AttachTo(NegativeThought thought);
}