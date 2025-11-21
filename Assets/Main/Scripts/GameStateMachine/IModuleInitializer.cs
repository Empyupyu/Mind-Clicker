using Cysharp.Threading.Tasks;

public interface IModuleInitializer
{
    UniTask InitializeModulesAsync();
}
