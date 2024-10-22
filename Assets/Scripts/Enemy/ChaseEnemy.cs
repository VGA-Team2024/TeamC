using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary> ある範囲に入ったらplayerを追いかける敵 </summary>
public class ChaseEnemy : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("空中か")] private bool _isFly;
    [SerializeField, Header("巡回する範囲")] private float _patrolArea;
    [SerializeField, Header("Playerにぶつかった後止まる時間")] private int _freezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    private SpriteRenderer _spriteRenderer;
    private CancellationToken _token;
    private ParticleSystem _particle;
    
    private EnemyWalkState _walkState;
    private EnemyChaseState _chaseState;
    private EnemyFreezeState _freezeState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerTransform = null;
        _token = this.GetCancellationTokenOnDestroy();
        _particle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        
        _walkState = new EnemyWalkState(this, transform, _speed, _patrolArea, transform.position, _spriteRenderer);
        _chaseState = new EnemyChaseState(this, transform, _speed, _spriteRenderer, _isFly);
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime, _token);
        _deathState = new EnemyDeathState(this, _particle, gameObject);
    }

    protected override void OnUpdate()
    {
        if (_playerTransform)
        {
            if (_currentState == _freezeState) return;
            if (_currentState != _chaseState)
            {
                ChangeState(_chaseState);
                return;
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
    
    public void GetPlayerMove(PlayerMove playerMove)
    {
        if (playerMove == null) return;
        _playerTransform = playerMove?.gameObject.transform;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_currentState == _freezeState) return;
        // todo:playerへのダメージはここ
        if(other.CompareTag(_playerTag) && other.TryGetComponent(out IDamageable dmg))
        {
            dmg.TakeDamage(1);
            ChangeState(_freezeState);
        }
    }
}
