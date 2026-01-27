using Cysharp.Threading.Tasks;
using Main.Scripts.Views;

public interface IMindLevelAnimator
{
    UniTask PlayLevelUpAnimation(MindView view, AudioPlayer audio, MindData data, MindLevelUpAnimation animation);
    UniTask PlayLevelReduceAnimation(MindView view, MindLevelUpAnimation animation);
    void KillAnimation(MindView view);
}
