using Cysharp.Threading.Tasks;
using Main.Scripts.Views;

public class MenuState : IGameState
{
    private readonly MenuView _menuView;

    public MenuState(MenuView menuView)
    {
        this._menuView = menuView;
    }

    public UniTask Enter()
    {
        _menuView.Show();

        return UniTask.CompletedTask;
    }

    public UniTask Exit()
    {
        _menuView.Hide();
        
        return UniTask.CompletedTask;
    }
}
