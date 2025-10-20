using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.Views
{
    public class UpgradeView : MonoBehaviour
    {
        [field: SerializeField] public Image Icon { get; private set; }
        [field: SerializeField] public Button Buy { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Description { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Level { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Price { get; private set; }
    }
}