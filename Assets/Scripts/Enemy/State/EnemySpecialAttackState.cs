using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemySpecialAttackState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly Animator _animator;
    private readonly int _attack = Animator.StringToHash("SpecialAttack");
    private readonly GameObject _attackCollider;
    
    public EnemySpecialAttackState(EnemyBase enemyBase, EnemyFreezeState freezeState, Animator animator, GameObject collider)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _animator = animator;
        _attackCollider = collider;
    }
    
    public void Enter()
    {
        if (_animator) _animator.SetTrigger(_attack);
        Attack().Forget();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }

    private async UniTask Attack()
    {
        _attackCollider.SetActive(true);
        await UniTask.WaitUntil(() => _attackCollider.activeSelf == false);
        _enemyBase.ChangeState(_freezeState);
    }
}
