using System.Collections.Generic;
using System.Linq;
using Zenject;

public class UpgradeInstaller : MonoInstaller
{
    [Inject] private List<UpgradeConfig> upgradeConfigs;

    public override void InstallBindings()
    {
        BindUpgradeEffects();
        Container.BindInterfacesAndSelfTo<Upgrade>().AsSingle();
        Container.BindInterfacesAndSelfTo<UpgradeController>().AsSingle();
        Container.BindInterfacesAndSelfTo<UpgradeSaveService>().AsSingle();
        Container.BindInterfacesAndSelfTo<UpgradeSoundFeedbackService>().AsSingle();
    }

    private void BindUpgradeEffects()
    {
        var configMap = upgradeConfigs.ToDictionary(c => c.Type, c => c);

        Container.Bind<IUpgradeEffect>().To<AddClickDamageEffect>().AsSingle()
       .WithArguments(configMap[UpgradeType.AddClickDamageEffect]);

        Container.Bind<IUpgradeEffect>().To<AddDamagePerSecondTiear1Effect>().AsSingle()
            .WithArguments(configMap[UpgradeType.AddDamagePerSecondTiear1Effect]);
    }
}
