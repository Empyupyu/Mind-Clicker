using System.Collections.Generic;
using Zenject;

public class ThoughtFactory : IInitializable
{
    private Dictionary<ThoughtType, IThoughtLogic> thoughtsDic;
    private readonly List<IThoughtLogic> thoughtLogics;

    public ThoughtFactory(List<IThoughtLogic> thoughtLogics)
    {
        this.thoughtLogics = thoughtLogics;
    }

    public void Initialize()
    {
        thoughtsDic = new Dictionary<ThoughtType, IThoughtLogic>();

        for (int i = 0; i < thoughtLogics.Count; i++)
        {
            IThoughtLogic logic = thoughtLogics[i];
            thoughtsDic.Add(logic.ThoughtType, logic);
        }
    }

    public NegativeThought GetThought(NegativeThoughtForm config)
    {
        NegativeThought thought = new NegativeThought(config.Id, config.Name, config.Health, config.Money);
        thoughtsDic.TryGetValue(config.ThoughtType, out IThoughtLogic thoughtLogic);

        if(thoughtLogic == null) { return thought; }

        thoughtLogic.AttachTo(thought);
        return thought;
    }
}
