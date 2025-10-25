using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BossCemeteryThoughtLogic : BossThoughtLogicBase
{
    private readonly CemeteryEnvironmentView cemeteryEnvironmentView;

    private CemeteryEnvironmentView instanceEnvironmentView;
    private Material currentSkybox;
    private Material transitionMaterial;

    public BossCemeteryThoughtLogic(ThoughtType thoughtType, BossFightPrepare bossFightPrepare, CemeteryEnvironmentView cemeteryEnvironmentView)
        : base(thoughtType, bossFightPrepare)
    {
        this.cemeteryEnvironmentView = cemeteryEnvironmentView;
    }

    public override void AttachTo(NegativeThought thought)
    {
        currentSkybox = RenderSettings.skybox;
        base.AttachTo(thought);

        InitializeEnvironment().Forget();
    }

    private async UniTask InitializeEnvironment()
    {
        instanceEnvironmentView = GameObject.Instantiate(cemeteryEnvironmentView);
        EnableGhosts(false);
        ChangeSkyBox(instanceEnvironmentView.ToSkybox);

        foreach (var mushroom in instanceEnvironmentView.Mushrooms)
        {
            mushroom.localScale = Vector3.zero;
        }

        foreach (var gravestone in instanceEnvironmentView.Gravestones)
        {
            gravestone.localScale = Vector3.zero;
        }

        foreach (var mushroom in instanceEnvironmentView.Mushrooms)
        {
            mushroom.DOScale(1, .4f).From(0).SetEase(Ease.OutBack);

            await UniTask.Delay(200);
        }

        foreach (var gravestone in instanceEnvironmentView.Gravestones)
        {
            gravestone.DOScale(1, 1).From(0).SetEase(Ease.OutBack);

            await UniTask.Delay(500);
        }

        EnableGhosts(true);

        bossFightPrepare.StartFight();
    }

    private void EnableGhosts(bool isActive)
    {
        foreach (var ghost in instanceEnvironmentView.Ghosts)
        {
            ghost.gameObject.SetActive(isActive);
        }
    }

    private void ChangeSkyBox(Material material)
    {
        Material targetMaterial = RenderSettings.skybox;
        transitionMaterial = new Material(RenderSettings.skybox);
        RenderSettings.skybox = transitionMaterial;

        DOTween.To(
            () => 0f,
            t =>
            {
                transitionMaterial.Lerp(targetMaterial, material, t);
            },
            1f,
            instanceEnvironmentView.Duration
        ).OnComplete(() =>
        {
            RenderSettings.skybox = material;
        });
    }

    protected override void OnTimerFinished()
    {
        GameObject.Destroy(instanceEnvironmentView.gameObject);
    }

    protected override void OnBossDeath(NegativeThought negativeThought)
    {
        base.OnBossDeath(negativeThought);
        ChangeSkyBox(currentSkybox);
        GameObject.Destroy(instanceEnvironmentView.gameObject);
    }
}
