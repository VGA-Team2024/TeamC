using System.Threading;
using UnityEngine;
using Random = System.Random;

/// <summary> ボスクマ </summary>
public class BossBear : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("接触時攻撃力")] private int _collideDamage;
    [SerializeField, Header("巡回する範囲")] private float _patrolArea;
    [SerializeField, Header("Playerを攻撃した後次の攻撃が可能になるまでの時間")] private int _freezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("敵の動き方")] private AnimationCurve _animationCurve;
    [SerializeField, Header("突進時の移動距離")] private float _rushDistance;
    [SerializeField, Header("突進時のスピード")] private float _rushSpeed;
    
    private CancellationToken _token;
    private ParticleSystem _particle;
    private Animator _animator;
    private GameObject _attackCollider;

    private EnemyWalkState _walkState;
    private EnemyAttackState _attackState;
    private EnemyJumpAttackState _jumpAttackState;
    private EnemyRushState _rushState;
    private EnemyFreezeState _freezeState;
    private EnemyDeathState _deathState;
    protected override void OnStart()
    {
        _particle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        _attackCollider = gameObject.transform.GetChild(2).gameObject;
        _animator = GetComponent<Animator>();
        
        _walkState = new EnemyWalkState(this, _animator, transform, _speed, _patrolArea);
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime, _token);
        _attackState = new EnemyAttackState(this, _freezeState, _animator, _attackCollider);
        _jumpAttackState = new EnemyJumpAttackState(this, _freezeState, _animator, transform, _animationCurve);
        _rushState = new EnemyRushState(this, _freezeState, _animator, transform, _rushDistance, _rushSpeed);
        _deathState = new EnemyDeathState(this, _particle, gameObject);
    }

    protected override void OnUpdate()
    {
        if (_isDeath) return;
        
        if (_playerMove)
        {
            if (_currentState == _attackState ||
                _currentState == _jumpAttackState ||
                _currentState == _rushState ||
                _currentState == _freezeState) return;
            
            transform.eulerAngles = new Vector2(0, _playerMove.transform.position.x > transform.position.x ? 0 : 180);
            
            var rnd = new Random().Next(0, 3);
            switch (rnd)
            {
                case 0:
                    ChangeState(_attackState);
                    break;
                case 1:
                    ChangeState(_jumpAttackState);
                    break;
                case 2:
                    ChangeState(_rushState);
                    break;
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
    
    private void OnTriggerStay(Collider other)
    {
        if (_currentState == _freezeState) return;
        if(other.CompareTag(_playerTag) && other.TryGetComponent(out IDamageable dmg))
        {
            dmg.TakeDamage(_collideDamage);
            ChangeState(_freezeState);
        }
    }
}
