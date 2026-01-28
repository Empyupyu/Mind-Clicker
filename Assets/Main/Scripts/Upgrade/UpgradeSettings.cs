using UnityEngine;

[CreateAssetMenu(menuName = "Create Game Datas/UpgradeSettings")]
public class UpgradeSettings : ScriptableObject
{
    [field: SerializeField] public int VisibleCount { get; private set; } = 7;
    [field: SerializeField] public float ItemHeight { get; private set; } = 200f;
}