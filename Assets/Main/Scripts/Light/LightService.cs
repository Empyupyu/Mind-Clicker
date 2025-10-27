using DG.Tweening;
using UnityEngine;

public class LightService
{
    private readonly Light directionLight;
    private readonly LightConfig originLightConfig;
    private readonly float originLightIntensity;
    private readonly Vector3 originSunRotate;

    private Tweener rotateTween;
    private Tweener intensityTween;
    private Tweener skyTween;
    private Tweener equatorTween;
    private Tweener groundTween;

    private Material transitionMaterial;

    public LightService(Light directionLight, LightConfig originLightConfig)
    {
        this.directionLight = directionLight;
        this.originLightConfig = originLightConfig;

        originLightIntensity = directionLight.intensity;
        originSunRotate = directionLight.transform.localEulerAngles;
    }

    public void SetLightIntensity(float endValue, float duration)
    {
        intensityTween?.Kill();

        intensityTween = directionLight.DOIntensity(endValue, duration);
    }

    public void ToOriginLightIntesity(float duration)
    {
        intensityTween?.Kill();

        intensityTween = directionLight.DOIntensity(originLightIntensity, duration);
    }

    public void ToOriginEnvironmentLighting()
    {
        SetEnvironmentLighting(originLightConfig);
    }

    public void SetEnvironmentLighting(LightConfig config)
    {
        skyTween?.Kill();
        equatorTween?.Kill();
        groundTween?.Kill();

        skyTween = DOTween.To(
            () => RenderSettings.ambientSkyColor,
            x => RenderSettings.ambientSkyColor = x,
            config.SkyColor,
            config.TransitionDuration
        );

        equatorTween = DOTween.To(
            () => RenderSettings.ambientEquatorColor,
            x => RenderSettings.ambientEquatorColor = x,
            config.EquatorColor,
            config.TransitionDuration
        );

        groundTween = DOTween.To(
            () => RenderSettings.ambientGroundColor,
            x => RenderSettings.ambientGroundColor = x,
            config.GroundColor,
            config.TransitionDuration
        );
    }

    public void SetSunRotate(Vector2 endValue, float duration)
    {
        rotateTween?.Kill();
        rotateTween = directionLight.transform.DOLocalRotate(endValue, duration);
    }

    public void ToOriginSunRotate(float duration)
    {
        rotateTween?.Kill();
        rotateTween = directionLight.transform.DOLocalRotate(originSunRotate, duration);
    }

    public void ChangeSkyBox(Material material, float duration)
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
            duration
        ).OnComplete(() =>
        {
            RenderSettings.skybox = material;
        });
    }
}
