using System.Collections.Generic;
using UnityEngine;

public class CemeteryEnvironmentView : MonoBehaviour
{
    [field: SerializeField] public List<Transform> Ghosts { get; private set; }
    [field: SerializeField] public List<Transform> Gravestones { get; private set; }
    [field: SerializeField] public List<Transform> Mushrooms { get; private set; }
    [field: SerializeField] public Material ToSkybox { get; private set; }
    [field: SerializeField] public float Duration { get; private set; } = 2f;

}