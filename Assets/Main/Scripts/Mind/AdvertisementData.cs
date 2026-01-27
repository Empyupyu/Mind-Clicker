using UnityEngine;

[CreateAssetMenu(fileName = "AdvertisementData", menuName = "Create Game Datas/Advertisement Data")]
public class AdvertisementData : ScriptableObject
{
    [field: SerializeField] public float InterstitialColdown { get; private set; } = 90f;
    [field: SerializeField] public bool ShowPreInterstitialView { get; private set; }
    [field: SerializeField] public float PreInterstitialViewDuration { get; private set; } = 4f;
}
