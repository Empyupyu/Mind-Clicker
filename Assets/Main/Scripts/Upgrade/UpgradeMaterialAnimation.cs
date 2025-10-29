using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class UpgradeMaterialAnimation
{
    private readonly CharacterView characterView;
    private readonly CharacterConfig characterConfig;
    private readonly Material material;
    private readonly Color baseColor;

    public UpgradeMaterialAnimation(CharacterView characterView, CharacterConfig characterConfig)
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

    private async UniTask ChangeColorGlasses(Color color, float duration, Ease ease)
    {
        await material.DOColor(color, duration).SetEase(ease).AsyncWaitForCompletion().AsUniTask();
    }

    //TODO
    public async UniTask Reduce()
    {
        characterView.Animator.SetTrigger("Despair");
        await UniTask.Delay(1500);
    }
}