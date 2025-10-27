using Cysharp.Threading.Tasks;
using Main.Scripts.Views;
using System;
using UnityEngine;
using Zenject;

public class MindController : IInitializable, IDisposable, ITickable
{
    private readonly Mind mind;
    private readonly MindView view;
    private readonly ThoughtSpawner spawner;
    private readonly IMindProgressService progress;
    private readonly IMindLevelUpService levelUp;

    public MindController(
        Mind mind,
        MindView view,
        ThoughtSpawner spawner,
        IMindProgressService progress,
        IMindLevelUpService levelUp)
    {
        this.mind = mind;
        this.view = view;
        this.spawner = spawner;
        this.progress = progress;
        this.levelUp = levelUp;
    }

    public void Initialize()
    {
        progress.Redraw();
        view.MindLevelText.text = (mind.Level + 1).ToString();

        spawner.OnDestroy += _ => progress.StartFarming();
        mind.OnLevelUp += () => levelUp.PlayLevelUp().Forget();
        mind.OnLevelReduce += () => view.MindLevelText.text = (mind.Level + 1).ToString();
    }

    public void Dispose()
    {
        spawner.OnDestroy -= _ => progress.StartFarming();
        mind.OnLevelUp -= () => levelUp.PlayLevelUp().Forget();
        mind.OnLevelReduce -= () => view.MindLevelText.text = (mind.Level + 1).ToString();
    }

    public void Tick()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            levelUp.PlayLevelUp().Forget();
        }
#endif
    }
}
