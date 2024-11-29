using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary> 敵のオブジェクト設置ステート </summary>
public class EnemyObjectCreateState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly Animator _animator;
    private readonly int _attack = Animator.StringToHash("Attack");
    private readonly Cottons _Cottons;
    
    public EnemyObjectCreateState(EnemyBase enemyBase, EnemyFreezeState freezeState,Animator animator, Cottons cottons)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _animator = animator;
        _Cottons = cottons;
    }
    
    public void Enter()
    {
        _Cottons.CreateObject();
        StopTimer().Forget();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
    
    private async UniTask StopTimer()
    {
        _animator.SetTrigger(_attack);
        await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        _enemyBase.ChangeState(_freezeState);
    }
}
