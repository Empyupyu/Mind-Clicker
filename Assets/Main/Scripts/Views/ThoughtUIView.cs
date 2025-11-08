using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtUIView : MonoBehaviour
{
    public SpawnPoint SpawnPoint { get; private set; }
    public NegativeThought Thought { get; private set; }
    [field : SerializeField] public Image Icon {  get; private set; }

    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    public void Initialize(NegativeThought thought, SpawnPoint SpawnPoint)
    {
        Thought = thought;
        this.SpawnPoint = SpawnPoint;
        healthBar.fillAmount = 1;
    }

    public void Redraw(NegativeThought thought)
    {
        healthBar.fillAmount = thought.CurrentHealth / thought.MaxHealth;
        healthText.text = thought.CurrentHealth.ToAbbreviatedString();
    }
}
