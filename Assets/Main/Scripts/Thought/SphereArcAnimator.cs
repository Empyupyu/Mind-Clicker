using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SphereArcAnimator : ISphereArcAnimator
{
    public void Animate(GameObject sphere, int i, int total, Vector3 pos, ThoughtSpawnPointData data)
    {
        float t = (float)i / (total - 1);
        float scale = data.BaseScale + Mathf.Pow(t, data.ScaleExponent);
        float duration = Mathf.Max(data.ScaleMinDuration, data.BaseDuration - t * data.DurationDecay);
        float delay = i * data.DelayStep;

        sphere.transform.DOScale(scale, duration).SetEase(Ease.OutBack).SetDelay(delay);

        float moveAmplitude = GetRandomValue(data.MoveAmplitudeMin, data.MoveAmplitudeMax);
        float moveDuration = GetRandomValue(data.MoveDurationMin, data.MoveDurationMax);

        sphere.transform.DOMoveY(pos.y + moveAmplitude, moveDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(delay + duration);
    }

    private float GetRandomValue(float min, float max)
    {
        return Random.Range(min, max);
    }
}
