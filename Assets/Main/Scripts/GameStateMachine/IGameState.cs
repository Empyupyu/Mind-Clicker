using Cysharp.Threading.Tasks;

public interface IGameState
{
    UniTask Enter();
    UniTask Exit();
}
