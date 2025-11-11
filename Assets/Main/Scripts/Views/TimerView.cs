using TMPro;
using UnityEngine;

public class TimerView : MonoBehaviour
{
    [field : SerializeField] public TextMeshProUGUI TimerText { get; private set; }

    public void Redraw(string value)
    {
        TimerText.text = value;
    }
}
