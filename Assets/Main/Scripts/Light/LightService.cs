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

    public void SetEnvironmentLighting(LightConfig config)
    {
        KillTweens();

        skyTween = AnimateAmbientColor(RenderSettings.ambientSkyColor, RenderSettings.ambientSkyColor, config.SkyColor, config.TransitionDuration);

        equatorTween = AnimateAmbientColor(RenderSettings.ambientSkyColor, RenderSettings.ambientSkyColor, config.EquatorColor, config.TransitionDuration);

        groundTween = AnimateAmbientColor(RenderSettings.ambientSkyColor, RenderSettings.ambientSkyColor, config.GroundColor, config.TransitionDuration);
    }

    public void SetSunRotate(Vector2 endValue, float duration)
    {
        rotateTween?.Kill();
        rotateTween = directionLight.transform.DOLocalRotate(endValue, duration);
    }

    public void ToOrigin()
    {
        ToOriginSunRotate();
        ToOriginLightIntesity();
        SetEnvironmentLighting(originLightConfig);
        ChangeSkyBox(originLightConfig.Skybox, originLightConfig.TransitionDuration);
    }

    public void ChangeSkyBox(Material material, float duration)
    {
        Material targetMaterial = RenderSettings.skybox;

        if (targetMaterial == material) return;

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

    private void ToOriginSunRotate()
    {
        SetSunRotate(originSunRotate, originLightConfig.TransitionDuration);
    }

    private void ToOriginLightIntesity()
    {
        intensityTween?.Kill();

        intensityTween = directionLight.DOIntensity(originLightIntensity, originLightConfig.TransitionDuration);
    }

    private void ToOriginEnvironmentLighting()
    {
        SetEnvironmentLighting(originLightConfig);
    }

    private void KillTweens()
    {
        skyTween?.Kill();
        equatorTween?.Kill();
        groundTween?.Kill();
    }

    private Tweener AnimateAmbientColor(Color getter, Color setter, Color target, float duration)
    {
        return DOTween.To(
            () => getter,
            x => setter = x,
            target,
            duration
        );
    }
}
