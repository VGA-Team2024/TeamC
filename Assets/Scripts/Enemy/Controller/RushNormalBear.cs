using UnityEngine;

/// <summary> ある範囲に入ったら突進する敵 </summary>
public class RushNormalBear : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("接触時攻撃力")] private int _collideDamage;
    [SerializeField, Header("巡回する範囲")] private float _patrolArea;
    [SerializeField, Header("Playerを攻撃した後次の攻撃が可能になるまでの時間")] private int _freezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("突進時の移動距離")] private float _rushDistance;
    [SerializeField, Header("突進時のスピード")] private float _rushSpeed;
    
    private ParticleSystem _particle;
    private Animator _animator;

    private EnemyWalkState _walkState;
    private EnemyRushState _rushState;
    private EnemyFreezeState _freezeState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        _particle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        _animator = gameObject.transform.GetChild(2).GetComponent<Animator>();
        
        _walkState = new EnemyWalkState(_animator, transform, _speed, _patrolArea);
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime);
        _rushState = new EnemyRushState(this, _freezeState, _animator, transform, _rushDistance, _rushSpeed);
        _deathState = new EnemyDeathState(this, _particle, _animator, gameObject);
    }

    protected override void OnUpdate()
    {
        if (_isDeath) return;
        
        if (_hp.CurrentHp <= 0)
        {
            _isDeath = true;
            ChangeState(_deathState);
            return;
        }
        
        if (_playerMove)
        {
            if (_currentState == _rushState || _currentState == _freezeState) return;
            
            transform.eulerAngles = new Vector2(0, _playerMove.transform.position.x > transform.position.x ? 0 : 180);
            ChangeState(_rushState);
        }
        else
        {
            if (_currentState == _idleState) ChangeState(_walkState);
        }
    }
    
    public void GetPlayerMove(PlayerMove playerMove)
    {
        _playerMove = playerMove;
    }
    
    private void OnCollisionStay(Collision other)
    {
        if (!other.gameObject.CompareTag(_playerTag)) return;
        if(other.gameObject.TryGetComponent(out IDamageable dmg) && other.gameObject.TryGetComponent(out IBlowable blo))
        {
            dmg.TakeDamage(_collideDamage);
            blo.BlownAway(transform.position);
            ChangeState(_freezeState);
        }
    }
}
