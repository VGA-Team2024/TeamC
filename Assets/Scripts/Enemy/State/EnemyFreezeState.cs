using System.Threading;
using Cysharp.Threading.Tasks;

public class EnemyFreezeState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyIdleState _idleState;
    private readonly int _freezeTime;
    private CancellationTokenSource _tokenSource;
    
    public EnemyFreezeState(EnemyBase enemyBase, EnemyIdleState idleState, int freezeTime)
    {
        _enemyBase = enemyBase;
        _idleState = idleState;
        _freezeTime = freezeTime;
    }
    
    public void Enter()
    {
        _tokenSource = new CancellationTokenSource();
        
        StartFreezeTimer().Forget();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        _tokenSource?.Cancel();
        _tokenSource?.Dispose();
    }
    
    private async UniTask StartFreezeTimer()
    {
        await UniTask.Delay(_freezeTime * 1000, cancellationToken:_tokenSource.Token);
        _enemyBase.ChangeState(_idleState);
    }
}
