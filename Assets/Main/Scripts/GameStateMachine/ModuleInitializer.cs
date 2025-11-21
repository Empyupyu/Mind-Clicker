using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class ModuleInitializer : IModuleInitializer
{
    private readonly IEnumerable<IGameModule> modules;

    public ModuleInitializer(IEnumerable<IGameModule> modules)
    {
        this.modules = modules;
    }

    public async UniTask InitializeModulesAsync()
    {
        var ordered = modules.OrderBy(m => m.Priority).ToList();
        foreach (var m in ordered)
            await m.InitializeAsync();
    }
}
