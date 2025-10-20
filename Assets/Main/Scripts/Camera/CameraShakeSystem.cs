using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeSystem : MonoBehaviour
{
    [SerializeField] private float amplitude = 0f;
    [SerializeField] private float frequency = 1f;
    [SerializeField] private float defaultShakeAmplitude = .5f;
    [SerializeField] private float defaultShakeFrequency = 10f;
    [SerializeField] private float shootDuration = .2f;

    private CinemachineBasicMultiChannelPerlin perlin;
    private Coroutine shaking;

    private void Start()
    {
        perlin = FindObjectOfType<CinemachineCamera>().gameObject.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        CameraReset();
    }

    public void ShakeCamera(float duration, float amplitude, float frequency)
    {
        if (shaking != null) return;

        duration = shootDuration == 0 ? duration : shootDuration;

        shaking = StartCoroutine(Shaking(duration, amplitude, frequency));
    }

    private IEnumerator Shaking(float duration, float amplitude, float frequency)
    {
        perlin.AmplitudeGain = amplitude;
        perlin.FrequencyGain = frequency;
        yield return new WaitForSeconds(duration);
        CameraReset();
    }

    private void CameraReset()
    {
        perlin.AmplitudeGain = amplitude;
        perlin.FrequencyGain = frequency;

        shaking = null;
    }
}
