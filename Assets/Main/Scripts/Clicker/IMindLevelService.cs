using Cysharp.Threading.Tasks;

public interface IMindLevelService
{
    UniTask LevelUp();
    UniTask LevelReduce();
}
