using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.Views
{
    public class UpgradeShopView : MonoBehaviour
    {
        [field: SerializeField] public UpgradeStateView UpgradeStateView { get; private set; }
        [field: SerializeField] public RectTransform ContentContainer { get; private set; }
        [field: SerializeField] public ScrollRect ScrollRect { get; private set; }
        [field: SerializeField] public Vector2 OffsetBetweenUpgradeView { get; private set; }
    }
}