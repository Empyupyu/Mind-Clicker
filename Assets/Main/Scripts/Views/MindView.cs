using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.Views
{
    public class MindView : MonoBehaviour
    {
        [field: SerializeField] public Image ProgressBar { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ProgressText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI MindLevelText { get; private set; }
    }
}