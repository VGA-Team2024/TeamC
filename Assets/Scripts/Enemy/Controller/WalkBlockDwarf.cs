using UnityEngine;

/// <summary> 循環のみする敵 </summary>
public class WalkBlockDwarf : EnemyBase
{
    [SerializeField, Header("巡回する範囲")] private float _patrolArea;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("Playerにぶつかった後止まる時間")] private int _freezeTime;
    [SerializeField, Header("Playerに与えるダメージ量")] private int _damage;

    private ParticleSystem _particle;
    private Animator _animator;

    private EnemyWalkState _walkState;
    private EnemyFreezeState _freezeState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        _particle = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();

        _walkState = new EnemyWalkState(_animator, transform, _speed, _patrolArea);
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime);
        _deathState = new EnemyDeathState(this, _particle, _animator, gameObject);
        ChangeState(_walkState);
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
        
        if (_currentState == _idleState) ChangeState(_walkState);
    }
   
    private void OnCollisionStay(Collision other)
    {
        if (!other.gameObject.CompareTag(_playerTag)) return;
        if(other.gameObject.TryGetComponent(out IDamageable dmg) && other.gameObject.TryGetComponent(out IBlowable blo))
        {
            dmg.TakeDamage(_damage);
            blo.BlownAway(transform.position);
            ChangeState(_freezeState);
        }
    }
    
    private void OnDrawGizmos()
    {
        if(Application.isPlaying) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + new Vector3(0, 1), Vector3.right * _patrolArea);
    }
}
