using UnityEngine;

public class PatrolMarionette : EnemyBase
{
    [SerializeField, Header("接触時攻撃力")] private int _collideDamage;
    [SerializeField, Header("巡回する範囲")] private float _patrolArea;
    [SerializeField, Header("ぶつかった後当たり判定がなくなる時間")] private int _freezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("親オブジェクト")] private GameObject _obj;

    private EnemyFreezeState _freezeState;
    private EnemyPatrolState _patrolState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        ParticleSystem particle = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        Animator animator = gameObject.transform.GetChild(1).GetComponent<Animator>();
        
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime);
        _patrolState = new EnemyPatrolState(animator, transform, _speed, _patrolArea);
        _deathState = new EnemyDeathState(this, particle, animator, _obj);
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
        
        if (_currentState == _idleState) ChangeState(_patrolState);
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
