using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary> 敵の攻撃ステート </summary>
public class EnemyAttackState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly Animator _animator;
    private readonly int _attack;
    private readonly GameObject _attackCollider;
    private bool _isAttack;
    
    public EnemyAttackState(EnemyBase enemyBase, EnemyFreezeState freezeState, 
        Animator animator, string animationName, GameObject collider)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _animator = animator;
        _attack = Animator.StringToHash(animationName);
        _attackCollider = collider;
    }
    
    public void Enter()
    {
        _isAttack = false;
    }

    public void Execute()
    {
        if (_isAttack) return;
        _isAttack = true;
        _attackCollider.SetActive(true);
        PlayAnim().Forget();
    }

    public void Exit()
    {
        _isAttack = false;
    }
    
    private async UniTask PlayAnim()
    {
        if (_animator) _animator.SetTrigger(_attack);
        await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        _enemyBase.ChangeState(_freezeState);
    }
}
