using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NegativeThoughtLevel", menuName = "Create Game Datas/NegativeThoughtLevel")]
public class NegativeThoughtLevel : ScriptableObject
{
    [field: SerializeField] public List<NegativeThoughtForm> NegativeThoughtForms { get; private set; }
}