using UnityEngine;

[CreateAssetMenu(fileName = "LightConfig", menuName = "Create Game Datas/LightConfig")]
public class LightConfig : ScriptableObject
{
    [field: SerializeField] public Material Skybox { get; private set; }
    [field: SerializeField] public float Intensity { get; private set; }
    [field: SerializeField] public Color SkyColor { get; private set; }
    [field: SerializeField] public Color EquatorColor { get; private set; }
    [field: SerializeField] public Color GroundColor { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; }
}