using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CutoutUIComponent : MonoBehaviour
{
    [SerializeField] private Vector3 targetSize = new Vector2(900, 900);
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image cirlceImage;
    [SerializeField] private float duration;

    private Vector3 circleScale;
    private Vector3 backgroundScale;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        circleScale = cirlceImage.transform.localScale;
        backgroundScale = backgroundImage.transform.localScale;
    }

    public void EndingTransition()
    {
        cirlceImage.transform.localScale = circleScale;
        backgroundImage.transform.localScale = backgroundScale;
        cirlceImage.rectTransform.DOSizeDelta(Vector2.zero, duration).From(targetSize);
    }

    public async UniTask OpeningTransition()
    {
        cirlceImage.transform.localScale = circleScale;
        backgroundImage.transform.localScale = backgroundScale;
        await cirlceImage.rectTransform.DOSizeDelta(targetSize, duration).From(Vector2.zero).OnComplete(() =>
        {
            gameObject.SetActive(false);
        }).AsyncWaitForCompletion().AsUniTask();
    }
}
