using Cysharp.Threading.Tasks;
using System.Threading;

public interface ILevelLoop
{
    UniTask StartGameAsync(CancellationToken token);
}
