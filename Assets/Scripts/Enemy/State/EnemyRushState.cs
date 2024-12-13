using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary> 敵の突進ステート </summary>
public class EnemyRushState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly Animator _animator;
    private readonly int _rush = Animator.StringToHash("Rush");
    private readonly Transform _transform;
    private readonly float _distance;
    private readonly float _speed;
    private float _destination;
    private bool _canRush;
    private CancellationTokenSource _tokenSource;

    public EnemyRushState(EnemyBase enemyBase, EnemyFreezeState freezeState, Animator animator,Transform transform, float dis, float speed)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _animator = animator;
        _transform = transform;
        _distance = dis;
        _speed = speed;
    }
    
    public void Enter()
    {
        _destination = _transform.position.x + _transform.right.x * _distance;
        _tokenSource = new CancellationTokenSource();
        Set().Forget();
    }

    public void Execute()
    {
        if (_canRush) Rush();
        if (Mathf.Abs(_transform.position.x - _destination) < 0.1f)
        {
            _enemyBase.ChangeState(_freezeState);
        }
    }

    public void Exit()
    {
        if (_animator) _animator.SetBool(_rush, false);
        _tokenSource?.Cancel();
        _tokenSource?.Dispose();
    }
    
    private async UniTask Set()
    {
        _canRush = false;
        if (_animator) _animator.SetBool(_rush, true);
        await UniTask.WaitUntil(() => _animator && _animator.GetCurrentAnimatorStateInfo(0).IsName("Rush2"), cancellationToken : _tokenSource.Token);
        _canRush = true;
    }

    private void Rush()
    { 
        _transform.Translate(Vector3.right * (Time.deltaTime * _speed));
    }
}