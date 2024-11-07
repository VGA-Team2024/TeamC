using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary> 循環のみする敵 </summary>
public class WalkBlockDwarf : EnemyBase
{
    [SerializeField, Header("巡回する範囲")] private float _patrolArea;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("Playerにぶつかった後止まる時間")] private int _freezeTime;
    [SerializeField, Header("Playerに与えるダメージ量")] private int _damage;

    private CancellationToken _token;
    private ParticleSystem _particle;
    private Animator _animator;

    private EnemyWalkState _walkState;
    private EnemyFreezeState _freezeState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        _token = this.GetCancellationTokenOnDestroy();
        _particle = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();

        _walkState = new EnemyWalkState(this, _animator, transform, _speed, _patrolArea);
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime, _token);
        _deathState = new EnemyDeathState(this, _particle, gameObject);
        ChangeState(_walkState);
    }

    protected override void OnUpdate()
    {
        if (_isDeath) return;
        
        if (_currentState == _idleState) ChangeState(_walkState);
        
        if (_hp.CurrentHp <= 0)
        {
            _isDeath = true;
            ChangeState(_deathState);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (_currentState == _freezeState) return;
        if(other.CompareTag(_playerTag) && other.TryGetComponent(out IDamageable dmg))
        {
            dmg.TakeDamage(_damage);
            ChangeState(_freezeState);
        }
    }
}