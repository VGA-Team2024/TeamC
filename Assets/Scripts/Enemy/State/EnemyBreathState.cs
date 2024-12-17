using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary> ドラゴンのブレス攻撃 </summary>
public class EnemyBreathState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly Animator _animator;
    private readonly int _breath = Animator.StringToHash("Breath");
    private readonly GameObject _breathObjs;
    
    public EnemyBreathState(EnemyBase enemyBase, EnemyFreezeState freezeState, Animator animator, GameObject breathObjs)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _animator = animator;
        _breathObjs = breathObjs;
    }

    public void Enter()
    {
        _breathObjs.SetActive(true);
        
        PlayAnim().Forget();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }

    private async UniTask PlayAnim()
    {
        if (_animator) _animator.SetTrigger(_breath);
        await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        _enemyBase.ChangeState(_freezeState);
    }
}
