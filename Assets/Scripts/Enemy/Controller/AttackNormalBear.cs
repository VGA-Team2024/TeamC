using UnityEngine;
using System.Threading;

/// <summary> ある範囲に入ったら攻撃をする敵 </summary>
public class AttackNormalBear : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("接触時攻撃力")] private int _collideDamage;
    [SerializeField, Header("巡回する範囲")] private float _patrolArea;
    [SerializeField, Header("Playerを攻撃した後次の攻撃が可能になるまでの時間")] private int _freezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    
    private CancellationToken _token;
    private ParticleSystem _particle;
    private Animator _animator;
    private GameObject _attackCollider;

    private EnemyWalkState _walkState;
    private EnemyAttackState _attackState;
    private EnemyFreezeState _freezeState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        _particle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        _attackCollider = gameObject.transform.GetChild(2).gameObject;
        _animator = gameObject.transform.GetChild(3).GetComponent<Animator>();
        
        _walkState = new EnemyWalkState(this, _animator, transform, _speed, _patrolArea);
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime, _token);
        _attackState = new EnemyAttackState(this, _freezeState, _animator, _attackCollider);
        _deathState = new EnemyDeathState(this, _particle, gameObject);
    }

    protected override void OnUpdate()
    {
        if (_isDeath) return;
        
        if (_playerMove)
        {
            if (_currentState == _attackState || _currentState == _freezeState) return;
            
            transform.eulerAngles = new Vector2(0, _playerMove.transform.position.x > transform.position.x ? 0 : 180);
            ChangeState(_attackState);
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
