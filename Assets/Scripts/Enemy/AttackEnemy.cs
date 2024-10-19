using UnityEngine;

/// <summary> ある範囲に入ったら攻撃をする敵 </summary>
public class AttackEnemy : EnemyBase
{
    [SerializeField, Header("攻撃力")] private int _power;

    private EnemyAttackState _attackState = default;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
