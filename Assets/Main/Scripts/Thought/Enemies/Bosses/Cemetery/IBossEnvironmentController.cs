using Cysharp.Threading.Tasks;

public interface IBossEnvironmentController
{
    UniTask Initialize(ThoughtType bossType);
    void Cleanup();
}
