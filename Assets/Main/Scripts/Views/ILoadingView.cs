using Cysharp.Threading.Tasks;

namespace Main.Scripts.Views
{
    public interface ILoadingView
    {
        void Show();
        UniTask Hide();
    }
}