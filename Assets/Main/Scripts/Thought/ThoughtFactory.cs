public class ThoughtFactory
{
    private readonly IThoughtHealthProvider thoughtHealthProvider;

    public ThoughtFactory(IThoughtHealthProvider thoughtHealthProvider)
    {
        this.thoughtHealthProvider = thoughtHealthProvider;
    }

    public NegativeThought GetThought(NegativeThoughtForm config, int level)
    {
        float hp = thoughtHealthProvider.CalculateHealth(config, level);
        NegativeThought thought = new NegativeThought(config.Id, config.Name, hp, config.Money);

        return thought;
    }
}
