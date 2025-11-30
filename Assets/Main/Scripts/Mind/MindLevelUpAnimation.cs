using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MindLevelUpAnimation : IMindLevelUpAnimation
{
    private readonly CharacterView characterView;
    private readonly CharacterConfig characterConfig;
    private readonly Material material;
    private readonly Color baseColor;

    public MindLevelUpAnimation(CharacterView characterView, CharacterConfig characterConfig)
    {
        this.characterView = characterView;
        this.characterConfig = characterConfig;
        material = characterView.SkinnedMeshRenderer.materials[1];
        baseColor = material.GetColor("_Color");
    }

    public async UniTask UpgradeColorAnimation()
    {
        await ChangeColorGlasses(Color.white, characterConfig.GlassesHighlightInDuration, characterConfig.GlassesHighlightInEase);
        await ChangeColorGlasses(baseColor, characterConfig.GlassesHighlightOutDuration, characterConfig.GlassesHighlightOutEase);
    }

    public async UniTask Reduce()
    {
        characterView.Animator.SetTrigger("Despair");
        await UniTask.Delay((int)(characterConfig.DespairDuration * 1000));
    }

    private async UniTask ChangeColorGlasses(Color color, float duration, Ease ease)
    {
        await material.DOColor(color, duration).SetEase(ease).AsyncWaitForCompletion().AsUniTask();
    }
}
