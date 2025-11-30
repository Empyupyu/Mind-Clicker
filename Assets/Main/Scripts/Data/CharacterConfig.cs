using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Create Game Datas/CharacterConfig")]
public class CharacterConfig : ScriptableObject
{
    [field: SerializeField] public Ease GlassesHighlightInEase { get; private set; }
    [field: SerializeField] public float GlassesHighlightInDuration { get; private set; }
    [field: SerializeField] public Ease GlassesHighlightOutEase { get; private set; }
    [field: SerializeField] public float GlassesHighlightOutDuration { get; private set; }
    [field: SerializeField] public float DespairDuration { get; private set; } = 1.5f;
}
