using UnityEngine;

/// <summary> 敵が死んだときのステート </summary>
public class EnemyDeathState : IEnemyState
{
    private EnemyBase _enemyBase = default;
    private GameObject _obj = default;
    
    /// <param name="obj"> 死んだときに生成する敵などがいたら入れる </param>
    public EnemyDeathState(EnemyBase enemyBase, GameObject obj = null)
    {
        _enemyBase = enemyBase;
        _obj = obj;
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
