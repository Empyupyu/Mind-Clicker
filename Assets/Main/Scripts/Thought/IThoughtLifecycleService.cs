using System;

public interface IThoughtLifecycleService
{
    public event Action<NegativeThought> OnDestroy;

    ThoughtUIView GetRandomView();
    NegativeThought GetTarget();
    void Register(NegativeThought thought, ThoughtUIView view);
    void HandleDeath(NegativeThought thought);
    void Unregister(NegativeThought thought);
    void UnregisterAll();
}
