using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossEnvironmentManifest", menuName = "Create Game Datas/BossEnvironmentManifest")]
public class BossEnvironmentManifest : ScriptableObject
{
    [field: SerializeField] public List<BossEnvironmentReference> Environments { get; private set; }
}
