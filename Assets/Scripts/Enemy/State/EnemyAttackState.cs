using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary> 敵の攻撃ステート </summary>
public class EnemyAttackState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly int _power;
    private PlayerMove _playerMove;
    private readonly string _playerTag;
    private readonly Animator _animator;
    private readonly int _attack;
    private bool _isAttack;
    
    public EnemyAttackState(EnemyBase enemyBase, EnemyFreezeState freezeState, int power, 
        Animator animator, string animationName, string playerTag)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _power = power;
        _animator = animator;
        _attack = Animator.StringToHash(animationName);
        _playerTag = playerTag;
    }
    
    public void Enter()
    {
        _isAttack = false;
    }

    public void Execute()
    {
        if (_isAttack) return;
        if(_playerMove.CompareTag(_playerTag) && _playerMove.TryGetComponent(out IDamageable dmg))
        {
            _isAttack = true;
            PlayAnim().Forget();
            dmg.TakeDamage(_power);
        }
    }

    public void Exit()
    {
        _isAttack = false;
    }

    public void GetPlayerTransform(PlayerMove playerMove)
    {
        _playerMove = playerMove;
    }

    private async UniTask PlayAnim()
    {
        if (_animator) _animator.SetTrigger(_attack);
        await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        _enemyBase.ChangeState(_freezeState);
    }
}
