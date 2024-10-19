using UnityEngine;

/// <summary> 敵の攻撃ステート </summary>
public class EnemyAttackState : IEnemyState
{
    private EnemyBase _enemyBase = default;
    private int _power = default;
    
    public EnemyAttackState(EnemyBase enemyBase, int power)
    {
        _enemyBase = enemyBase;
        _power = power;
    }
    
    public void Enter()
    {
        Debug.Log("攻撃開始");
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        Debug.Log("攻撃終了");
    }
}
