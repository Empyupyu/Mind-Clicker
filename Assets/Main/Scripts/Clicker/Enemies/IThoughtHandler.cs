public interface IThoughtHandler
{
    public ThoughtType ThoughtType { get;}
    public void AttachTo(NegativeThought thought);
}