using System.Threading;
using Cysharp.Threading.Tasks;

public class EnemyFreezeState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyIdleState _idleState;
    private readonly int _freezeTime;
    private readonly CancellationToken _token;
    
    public EnemyFreezeState(EnemyBase enemyBase, EnemyIdleState idleState, int freezeTime, CancellationToken token)
    {
        _enemyBase = enemyBase;
        _idleState = idleState;
        _freezeTime = freezeTime;
        _token = token;
    }
    
    public void Enter()
    {
        StartFreezeTimer().Forget();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
    
    private async UniTask StartFreezeTimer()
    {
        await UniTask.Delay(_freezeTime * 1000, cancellationToken:_token);
        _enemyBase.ChangeState(_idleState);
    }
}
