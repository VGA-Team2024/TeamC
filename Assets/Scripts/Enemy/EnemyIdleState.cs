using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    private EnemyBase _enemyBase = default;
    
    public EnemyIdleState(EnemyBase enemyBase)
    {
        _enemyBase = enemyBase;
    }
    
    public void Enter()
    {
        
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}
