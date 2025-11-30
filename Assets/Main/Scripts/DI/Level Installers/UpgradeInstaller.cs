using System.Collections.Generic;
using System.Linq;
using Zenject;

public class UpgradeInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindUpgradeEffects();
        Container.Bind<MindLevelUpAnimation>().AsSingle();
        Container.Bind<IUpgradeViewFactory>().To<UpgradeViewFactory>().AsSingle();
        Container.Bind<IUpgradeCalculator>().To<UpgradeCalculator>().AsSingle();
        Container.BindInterfacesAndSelfTo<UpgradeService>().AsSingle();
        Container.BindInterfacesAndSelfTo<UpgradeController>().AsSingle();
        Container.BindInterfacesAndSelfTo<UpgradeSaveService>().AsSingle();
        Container.BindInterfacesAndSelfTo<UpgradeSoundFeedbackService>().AsSingle();
    }

    private void BindUpgradeEffects()
    {
        Container.Bind<IUpgradeEffect>().To<AddClickDamageEffect>().AsSingle()
       .WithArguments(UpgradeType.AddClickDamageEffect);

        Container.Bind<IUpgradeEffect>().To<AddDamagePerSecondEffect>().AsSingle()
            .WithArguments(UpgradeType.AddDamagePerSecondEffect);
    }
}
