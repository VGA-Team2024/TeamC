using UnityEngine;

/// <summary> ある範囲に入ったらplayerを追いかける敵 </summary>
public class ChaseEnemy : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("空中か")] private bool _isFly;
    [SerializeField, Header("巡回する範囲")] private float _patrolArea;
    private SpriteRenderer _spriteRenderer = default;
    private Transform _playerTransform = default;
    
    private EnemyWalkState _walkState = default;
    private EnemyChaseState _chaseState = default;
    private EnemyDeathState _deathState = default;
    
    protected override void OnStart()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerTransform = null;
        
        _walkState = new EnemyWalkState(this, transform, _speed, _patrolArea, transform.position, _spriteRenderer);
        _chaseState = new EnemyChaseState(this, transform, _speed, _spriteRenderer, _isFly);
        _deathState = new EnemyDeathState(this);
    }

    protected override void OnUpdate()
    {
        if (_playerTransform)
        {
            if (_currentState != _chaseState)
            {
                ChangeState(_chaseState);
                return;
            }
            if ((transform.position - _playerTransform.position).sqrMagnitude < 2f)
            {
                // ToDo:playerへの攻撃(ぶつかったらダメージを食らう場合)
            }
            _chaseState.GetPlayerPos(_playerTransform.position);
        }
        else
        {
            if (_currentState != _walkState) ChangeState(_walkState);
        }

        if (_hp.CurrentHp <= 0)
        {
            if (_isDeath) return;
            
            _isDeath = true;
            ChangeState(_deathState);
        }
    }
    
    public void GetPlayerTransform(Transform trans)
    {
        _playerTransform = trans;
    }
}
