using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class AdvertisementRewardButton : MonoBehaviour
{
    [SerializeField] private AdvertisementRewardID id;

    [Inject] private SignalBus signalBus;

    private Button button;

    public void SetID(AdvertisementRewardID id)
    {
        this.id = id;
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
            signalBus.Fire(new AdvertisementRewardButtonClickedSignal(id)));

        signalBus.Subscribe<RewardCooldownUpdatedSignal>(OnCooldownUpdated);
    }

    private void OnEnable()
    {
        signalBus.Fire(new RewardButtonInitializedSignal(id, this));
    }

    private void OnCooldownUpdated(RewardCooldownUpdatedSignal signal)
    {
        if (signal.RewardId == id)
            button.interactable = signal.IsAvailable;
    }

    private void OnDestroy()
    {
        signalBus.Unsubscribe<RewardCooldownUpdatedSignal>(OnCooldownUpdated);
    }
}
