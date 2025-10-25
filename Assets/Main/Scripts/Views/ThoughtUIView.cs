using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtUIView : MonoBehaviour
{
    public SphereArcSpawner SphereArcSpawner { get; private set; }
    public NegativeThought Thought { get; private set; }
    [field : SerializeField] public Image Icon {  get; private set; }

    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    public void Initialize(NegativeThought thought, SphereArcSpawner sphereArcSpawner)
    {
        Thought = thought;
        SphereArcSpawner = sphereArcSpawner;
        healthBar.fillAmount = 1;
    }

    public void Redraw(NegativeThought thought)
    {
        healthBar.fillAmount = thought.CurrentHealth / thought.MaxHealth;
        healthText.text = thought.CurrentHealth.ToAbbreviatedString();
    }
}
