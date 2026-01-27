using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AdvertisementRewardButton : MonoBehaviour
{
    [field: SerializeField] public AdvertisementRewardID ID { get; private set; }
    [field: SerializeField] public Button Button { get; private set; }

    public void SetID(AdvertisementRewardID id)
    {
        ID = id;
    }

    public void SetInteractable(bool interactable)
    {
        Button.interactable = interactable;
    }
}
