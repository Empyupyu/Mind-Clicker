using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SphereArcAnimator : ISphereArcAnimator
{
    public void Animate(GameObject sphere, int i, int total, Vector3 pos, ThoughtSpawnPointData data)
    {
        float t = (float)i / (total - 1);
        float scale = data.BaseScale + Mathf.Pow(t, data.ScaleExponent);
        float duration = Mathf.Max(0.2f, data.BaseDuration - t * data.DurationDecay);
        float delay = i * data.DelayStep;

        sphere.transform.DOScale(scale, duration).SetEase(Ease.OutBack).SetDelay(delay);

        float floatAmplitude = Random.Range(0.05f, 0.15f);
        float floatDuration = Random.Range(1.5f, 3f);

        sphere.transform.DOMoveY(pos.y + floatAmplitude, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(delay + duration);
    }
}
