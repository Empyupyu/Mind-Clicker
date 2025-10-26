using UnityEngine;

[CreateAssetMenu(fileName = "ProjectSettingsConfig", menuName = "Create Game Datas/ProjectSettingsConfig")]
public class ProjectSettingsConfig : ScriptableObject
{
    [field: SerializeField] public PlayerDataProviderType PlayerDataProviderType { get; private set; }
}