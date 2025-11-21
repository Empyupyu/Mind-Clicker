using DG.Tweening;
using UnityEngine;
using Zenject;

public class Popup : MonoBehaviour
{
    [field: SerializeField] public AudioClip InClip { get; private set; }
    [field: SerializeField] public AudioClip EndClip { get; private set; }

    [SerializeField] private float InScale, EndScale;
    [SerializeField] private float InDuration, EndDuration;
    [SerializeField] private Ease InEase, EndEase;
   
    [Inject] private SignalBus signalBus;

    public void Show()
    {
        transform.localScale = Vector3.zero;
        signalBus.Fire(new SoundEffectSignal(InClip, 1));

        gameObject.SetActive(true);
        transform.DOScale(InScale, InDuration).SetEase(InEase);
    }

    public void Hide()
    {
        transform.DOScale(EndScale, EndDuration).SetEase(EndEase).OnComplete(() =>
        {
            gameObject.SetActive(false);
            signalBus.Fire(new SoundEffectSignal(EndClip, 1));
        });
    }
}
