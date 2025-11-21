using Zenject;

public class LocalInstaller : Installer<LocalInstaller>
{
    private ProjectSettingsConfig projectSettingsConfig;

    public LocalInstaller(ProjectSettingsConfig config)
    {
        this.projectSettingsConfig = config;
    }

    public override void InstallBindings()
    {
       
    }
}
