using UnityEngine;

namespace Main.Scripts.Views
{
    public class UpgradeShopView : MonoBehaviour
    {
        [field: SerializeField] public UpgradeStateView UpgradeStateView { get; private set; }
        [field: SerializeField] public Transform ContentContainer { get; private set; }
        [field: SerializeField] public Vector2 OffsetBetweenUpgradeView { get; private set; }
    }
}