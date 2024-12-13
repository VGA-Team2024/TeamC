using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary> 敵の追跡ステート </summary>
public class EnemyChaseState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly Animator _animator;
    private readonly int _chase = Animator.StringToHash("Chase");
    private readonly Transform _transform;
    private readonly float _speed;
    private Vector2 _playerPos;
    private readonly bool _isFly;
    private CancellationTokenSource _tokenSource;
    private readonly int _time;
    
    public EnemyChaseState(EnemyBase enemyBase,EnemyFreezeState freezeState, Animator animator, Transform transform, float speed, bool isFly, int time = 0)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _animator = animator;
        _transform = transform;
        _speed = speed;
        _isFly = isFly;
        _time = time;
    }
    
    public void Enter()
    {
        if (_animator) _animator.SetBool(_chase, true);
        if (_time != 0)
        {
            _tokenSource = new CancellationTokenSource();
            ChaseTimer().Forget();
        }
    }

    public void Execute()
    {
        Chase();
        Direction();
    }

    public void Exit()
    {
        _animator.SetBool(_chase, false);
        if (_time != 0)
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
        }
    }

    public void GetPlayerPos(Vector2 pos)
    {
        _playerPos = pos;
    }

    private void Chase()
    {
        _transform.position = 
            Vector2.MoveTowards(_transform.position, _isFly ? _playerPos : new Vector2(_playerPos.x, _transform.position.y), _speed * Time.deltaTime);
    }

    private void Direction()
    {
        _transform.eulerAngles = new Vector2(0, _playerPos.x > _transform.position.x ? 0 : 180);
    }
    
    private async UniTask ChaseTimer()
    {
        await UniTask.Delay(_time * 1000, cancellationToken:_tokenSource.Token);
        _enemyBase.ChangeState(_freezeState);
    }
}
