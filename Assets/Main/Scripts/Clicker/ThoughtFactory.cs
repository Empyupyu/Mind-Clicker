using System.Collections.Generic;
using Zenject;

public class ThoughtFactory : IInitializable
{
    private Dictionary<ThoughtType, IThoughtHandler> thoughtsDic;
    private readonly List<IThoughtHandler> thoughtLogics;
    private readonly IThoughtHealthProvider thoughtHealthProvider;

    public ThoughtFactory(List<IThoughtHandler> thoughtLogics, IThoughtHealthProvider thoughtHealthProvider)
    {
        this.thoughtLogics = thoughtLogics;
        this.thoughtHealthProvider = thoughtHealthProvider;
    }

    public void Initialize()
    {
        thoughtsDic = new Dictionary<ThoughtType, IThoughtHandler>();

        for (int i = 0; i < thoughtLogics.Count; i++)
        {
            IThoughtHandler logic = thoughtLogics[i];
            thoughtsDic.Add(logic.ThoughtType, logic);
        }
    }

    public NegativeThought GetThought(NegativeThoughtForm config, int level)
    {
        float hp = thoughtHealthProvider.CalculateHealth(config, level);
        NegativeThought thought = new NegativeThought(config.Id, config.Name, hp, config.Money);
        thoughtsDic.TryGetValue(config.ThoughtType, out IThoughtHandler thoughtLogic);

        if(thoughtLogic == null) { return thought; }

        thoughtLogic.AttachTo(thought);
        return thought;
    }
}