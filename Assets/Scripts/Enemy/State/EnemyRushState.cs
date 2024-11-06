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
        if (_animator) _animator.SetTrigger(_rush);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}
