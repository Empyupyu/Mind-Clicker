using TMPro;
using UnityEngine;

public class AdvertisementTimerView : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI TimerText { get; private set; }
    [field: SerializeField] public Popup Popup { get; private set; }

    public void Redraw(string value)
    {
        TimerText.text = value;
    }
}
