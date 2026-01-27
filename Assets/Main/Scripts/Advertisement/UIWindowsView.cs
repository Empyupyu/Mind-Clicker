using UnityEngine;
using UnityEngine.UI;

public class UIWindowsView : MonoBehaviour
{
    [field: SerializeField] public Button ShopButton { get; private set; }
    [field: SerializeField] public GameObject LoadingView { get; private set; }
}