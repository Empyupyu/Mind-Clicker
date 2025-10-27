using System.Collections.Generic;
using UnityEngine;

public class CemeteryEnvironmentView : MonoBehaviour
{
    [field: SerializeField] public List<Transform> Ghosts { get; private set; }
    [field: SerializeField] public List<Transform> Gravestones { get; private set; }
    [field: SerializeField] public List<Transform> Mushrooms { get; private set; }
    [field: SerializeField] public LightConfig LightConfig { get; private set; }
    [field: SerializeField] public float Duration { get; private set; } = 2f;
    [field: SerializeField] public AudioClip AudioClip { get; private set; }

}