using UnityEngine;
using System.Threading;

/// <summary> ある範囲に入ったら攻撃をする敵 </summary>
public class AttackEnemy : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("攻撃力")] private int _power;
    [SerializeField, Header("巡回する範囲")] private float _patrolArea;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("Triggerの名前")] private string _animation;
    [SerializeField, Header("Playerを攻撃した後次の攻撃が可能になるまでの時間")] private int _freezeTime;
    private SpriteRenderer _spriteRenderer;
    private CancellationToken _token;
    private PlayerMove _playerMove;
    private ParticleSystem _particle;
    private Animator _animator;

    private EnemyWalkState _walkState;
    private EnemyAttackState _attackState;
    private EnemyFreezeState _freezeState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        _particle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        
        _walkState = new EnemyWalkState(this, transform, _speed, _patrolArea, transform.position, _spriteRenderer);
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime, _token);
        _attackState = new EnemyAttackState(this, _freezeState, _power, _animator, _animation, _playerTag);
        _deathState = new EnemyDeathState(this, _particle, gameObject);
    }

    protected override void OnUpdate()
    {
        if (_isDeath) return;
        
        if (_playerMove)
        {
            if (_currentState == _idleState || _currentState == _walkState)
            {
                _attackState.GetPlayerTransform(_playerMove);
                ChangeState(_attackState);
            }
        }
        else
        {
            if (_currentState == _idleState) ChangeState(_walkState);
        }
        
        if (_hp.CurrentHp <= 0)
        {
            _isDeath = true;
            ChangeState(_deathState);
        }
    }
    
    public void GetPlayerMove(PlayerMove playerMove)
    {
        _playerMove = playerMove;
    }
}
