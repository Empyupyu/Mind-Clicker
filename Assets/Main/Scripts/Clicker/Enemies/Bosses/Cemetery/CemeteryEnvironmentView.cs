using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CemeteryEnvironmentView : MonoBehaviour, IBossEnvironmentView
{
    [field: SerializeField] public LightConfig LightConfig { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; } = 2f;
    [field: SerializeField] public AudioClip AudioClip { get; private set; }
    [field: SerializeField] public List<Transform> Ghosts { get; private set; }
    [field: SerializeField] public List<Transform> Gravestones { get; private set; }
    [field: SerializeField] public List<Transform> Mushrooms { get; private set; }

    public async UniTask ApplyAnimation()
    {
        SetPropsScale(Mushrooms, Vector3.zero);
        SetPropsScale(Gravestones, Vector3.zero);
        EnableGhosts(false);

        await AnimateProps(Mushrooms, 1f, 0.4f, Ease.OutBack, 200);
        await AnimateProps(Gravestones, 1f, 1f, Ease.OutBack, 500);

        EnableGhosts(true);
    }

    private void EnableGhosts(bool active)
    {
        foreach (var ghost in Ghosts)
            ghost.gameObject.SetActive(active);
    }

    private void SetPropsScale(List<Transform> props, Vector3 scale)
    {
        foreach (var prop in props)
            prop.localScale = scale;
    }

    private async UniTask AnimateProps(List<Transform> props, float scale, float duration, Ease ease, int delay)
    {
        foreach (var prop in props)
        {
            prop.DOScale(scale, duration).From(0).SetEase(ease);
            await UniTask.Delay(delay);
        }
    }
}