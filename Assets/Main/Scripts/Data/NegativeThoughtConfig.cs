using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NegativeThoughtConfig", menuName = "Create Game Datas/NegativeThoughtConfig")]
public class NegativeThoughtConfig : ScriptableObject
{
    [field: SerializeField] public List<NegativeThoughtLevel> NegativeThoughtLevels { get; private set; }
    [field: SerializeField] public int MaxThoughtsInGame { get; private set; } = 5;
}