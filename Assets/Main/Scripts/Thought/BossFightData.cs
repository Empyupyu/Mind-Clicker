using UnityEngine;

[CreateAssetMenu(fileName = "BossFightData", menuName = "Create Game Datas/Boss Fight Data")]
public class BossFightData : ScriptableObject
{
    [field: SerializeField] public float Duration { get; private set; } = 60f;
}