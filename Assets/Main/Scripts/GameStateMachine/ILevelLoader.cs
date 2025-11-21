using Cysharp.Threading.Tasks;

public interface ILevelLoader
{
    UniTask LoadLevelAsync(string key);
}
