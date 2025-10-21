using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.VisualScripting.Antlr3.Runtime;

public class EyeController : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer m_Renderer;
    [SerializeField] private float rotateSpeed = 5;
    [SerializeField] private float eyeballToOrigin = .15f;
    [SerializeField] private float eyelidSpeed = 1;
    [SerializeField] private float closeEyeSpeedOnDamage = .3f;
    [SerializeField] private float openEyeSpeedOnDamage = .3f;
    [SerializeField] private Ease closeEyeEaseOnDamage;
    [SerializeField] private Ease closeEyeEaseIdle;
    [SerializeField] private Ease openEyeEaseIdle;
    [SerializeField] private Ease openEyeEaseOnDamage;
    [SerializeField] private Ease eyeballToOriginEase;
    [SerializeField] private Transform character;
    [SerializeField] private Transform TopEyelid;
    [SerializeField] private Transform LowerEyelid;
    [SerializeField] private Transform Eyeball;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 FullOpenTopRotateEyelid;
    [SerializeField] private Vector3 FullOpenDownRotateEyelid;
    [SerializeField] private Vector3 CloseTopRotateEyelid;
    [SerializeField] private Vector3 CloseDownRotateEyelid;
    [SerializeField] private Vector3 originEyeballRotation;
    [SerializeField] private float minX = -30f;
    [SerializeField] private float maxX = 30f;
    [SerializeField] private float minY = -45f;
    [SerializeField] private float maxY = 45f;
    [SerializeField] private Ease ease;

    private CancellationTokenSource damageDelayCts;
    private CancellationTokenSource idleCts;
    private bool eyesClosed = false;

    private void Awake()
    {
        originEyeballRotation = Eyeball.localEulerAngles;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenEyes(eyelidSpeed, openEyeEaseIdle);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CloseEyes(eyelidSpeed, closeEyeEaseIdle);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ScaleEyeball();

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            StartIdleAnimation();

        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            DamageReaction().Forget();

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Eyeball.DOLookAt(character.transform.position + offset, rotateSpeed);
        }
    }

    private void ScaleEyeball(float value = -1)
    {
        int index = m_Renderer.sharedMesh.GetBlendShapeIndex("Center");

        if (index >= 0)
        {
            if(value == -1)
            {
                value = m_Renderer.GetBlendShapeWeight(index);

                if (value != 32.2f)
                {
                    value = 32.2f;
                }
                else
                {
                    value = 0;
                }
            }

            m_Renderer.SetBlendShapeWeight(index, value);
        }
    }

    public void StartIdleAnimation()
    {
        idleCts?.Cancel();
        idleCts?.Dispose();
        idleCts = new CancellationTokenSource();

        _ = IdleAnimation(idleCts.Token);
    }

    private async UniTaskVoid IdleAnimation(CancellationToken token)
    {
        try
        {
            int countMoing = Random.Range(4, 10);

            for (int i = 0; i < countMoing; i++)
            {
                await Eyeball.DOLocalRotate(new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0), rotateSpeed).SetEase(ease).AsyncWaitForCompletion().AsUniTask().AttachExternalCancellation(token);

                var delay = Random.Range(0, 3);

                if (delay == 1)
                {
                    await UniTask.Delay(Random.Range(100, 500), cancellationToken: token);
                }

                delay = Random.Range(0, 2);

                if (delay == 1)
                {
                    ScaleEyeball();
                }
            }

            await CloseEyes(eyelidSpeed, closeEyeEaseIdle).AttachExternalCancellation(token);
            await UniTask.Delay(300, cancellationToken: token);
            await OpenEyes(eyelidSpeed, openEyeEaseIdle).AttachExternalCancellation(token);

            StartIdleAnimation();
        }
        catch
        {

        }
    }

    public async UniTask DamageReaction()
    {
        idleCts?.Cancel();
        idleCts?.Dispose();
        idleCts = null;

        if (!eyesClosed)
        {
            Eyeball.DOKill();
            await Eyeball.DOLocalRotate(originEyeballRotation, eyeballToOrigin).SetEase(eyeballToOriginEase).AsyncWaitForCompletion().AsUniTask();
            
            ScaleEyeball(32.2f);
            _ = CloseEyes(closeEyeSpeedOnDamage, closeEyeEaseOnDamage);
            eyesClosed = true;
        }

        damageDelayCts?.Cancel();
        damageDelayCts?.Dispose();
        damageDelayCts = new CancellationTokenSource();

        _ = WaitAndOpenEyes(damageDelayCts.Token);
    }

    private async UniTaskVoid WaitAndOpenEyes(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(1500, cancellationToken: token);
            await OpenEyes(openEyeSpeedOnDamage, openEyeEaseOnDamage);
            await UniTask.Delay(200);
            ScaleEyeball();
            eyesClosed = false;
            await UniTask.Delay(150);
            StartIdleAnimation();
        }
        catch 
        {
            // Таймер сброшен — ничего не делаем
        }
    }

    private async UniTask OpenEyes(float animateSpeed, Ease ease)
    {
        TopEyelid.DOLocalRotate(FullOpenTopRotateEyelid, animateSpeed, RotateMode.Fast).SetEase(ease);
        await LowerEyelid.DOLocalRotate(FullOpenDownRotateEyelid, animateSpeed, RotateMode.Fast).SetEase(ease).AsyncWaitForCompletion().AsUniTask();
    }

    private async UniTask CloseEyes(float animateSpeed, Ease ease)
    {
        TopEyelid.DOLocalRotate(CloseTopRotateEyelid, animateSpeed).SetEase(ease);
       await LowerEyelid.DOLocalRotate(CloseDownRotateEyelid, animateSpeed).SetEase(ease).AsyncWaitForCompletion().AsUniTask();
    }
}
