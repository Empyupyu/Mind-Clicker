using UnityEngine;

[CreateAssetMenu(fileName = "SphereArcConfig", menuName = "Create Game Datas/SphereArcConfig")]
public class SphereArcConfig : ScriptableObject
{
    [Header("Disable Settings")]
    public float DisableScaleDuration = 0.2f;

    [Header("View Settings")]
    public float ViewAppearDelay = 1f;
    public float ViewScale = 0.00053f;
    public float ViewScaleOnStart = 0f;
    public float ViewScaleDuration = 1f;
    public float CameraZOffset = 0.58f;
    public float MinScreenZ = 0.1f;

    [Header("Arc Settings")]
    public float DefaultDelayStep = 0.1f;
}