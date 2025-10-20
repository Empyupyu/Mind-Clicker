using Cysharp.Threading.Tasks;

namespace Main.Scripts.Views
{
    public interface IGameStateView
    {
        void Show();
        UniTask Hide();
    }
}