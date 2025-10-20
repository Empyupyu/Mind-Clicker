using UnityEngine;

[CreateAssetMenu(fileName = "NegativeThoughtForm", menuName = "Create Game Datas/NegativeThoughtForm")]
public class NegativeThoughtForm : ScriptableObject
{
    [field: SerializeField] public ThoughtType ThoughtType { get; private set; }
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public float Health { get; private set; }
    [field: SerializeField] public float Money { get; private set; }
    [field: SerializeField] public GameObject Head { get; private set; }
    [field: SerializeField] public GameObject Body { get; private set; }
}                                                   
                                                    
                                                    