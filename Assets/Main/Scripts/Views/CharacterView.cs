using UnityEngine;

public class CharacterView : MonoBehaviour 
{
    [field: SerializeField] public SkinnedMeshRenderer SkinnedMeshRenderer { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
}
