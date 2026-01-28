using UnityEngine;

[CreateAssetMenu(menuName = "Create Game Datas/UpgradeConfig 1")]
public class UpgradeConfig : ScriptableObject
{
    [field: SerializeField] public UpgradeType Type { get; private set; }

    [Header("Economy")]
    [field: SerializeField] public float BasePrice { get; private set; } = 100f;
    [field: SerializeField] public float PriceMultiplier { get; private set; } = 1.19f;

    [field: SerializeField] public float BaseEffect { get; private set; } = 1f;
    [field: SerializeField] public float EffectMultiplier { get; private set; } = 1.17f;

    [Header("UI")]
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public string DescriptionPrefix { get; private set; }
    [field: SerializeField] public string DescriptionSuffix { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}
