using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemySpecialAttackState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly Animator _animator;
    private readonly int _attack;
    private readonly GameObject _attackCollider;
    private readonly float _time;
    private CancellationTokenSource _tokenSource;
    
    public EnemySpecialAttackState(EnemyBase enemyBase, EnemyFreezeState freezeState, Animator animator, GameObject collider, float time, string _animName)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _animator = animator;
        _attackCollider = collider;
        _attack = Animator.StringToHash(_animName);
        _time = time;
    }
    
    public void Enter()
    {
        _tokenSource = new CancellationTokenSource();
        if (_animator) _animator.SetTrigger(_attack);
        Attack().Forget();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        _tokenSource?.Cancel();
        _tokenSource?.Dispose();
    }

    private async UniTask Attack()
    {
        await UniTask.Delay((int)_time * 1000, cancellationToken: _tokenSource.Token);
        _attackCollider.SetActive(true);
        await UniTask.WaitUntil(() => _attackCollider.activeSelf == false);
        _enemyBase.ChangeState(_freezeState);
    }
}
