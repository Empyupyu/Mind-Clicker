using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Main.Scripts.Views
{
    public class LoadingView : MonoBehaviour, IGameStateView
    {
        [SerializeField] private CutoutUIComponent cutoutUIComponent;

        public void Show()
        {
            cutoutUIComponent.gameObject.SetActive(true);
        }

        public async UniTask Hide()
        {
            await cutoutUIComponent.OpeningTransition();
        }
    }
}