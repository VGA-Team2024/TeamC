using UnityEngine;

/// <summary> 追跡のみする敵 </summary>
public class ChaseBlockDwarf : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("空中か")] private bool _isFly;
    [SerializeField, Header("Playerにぶつかった後止まる時間")] private int _freezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("Playerに与えるダメージ量")] private int _damage;
    [SerializeField, Header("索敵をやめる距離")] private float _canselDis;

    private ParticleSystem _particle;
    private Animator _animator;
    
    private EnemyChaseState _chaseState;
    private EnemyFreezeState _freezeState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        _particle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();

        _chaseState = new EnemyChaseState(this, _freezeState, _animator, transform, _speed, _isFly);
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime);
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
            if ((transform.position - _playerMove.transform.position).sqrMagnitude > _canselDis * _canselDis)
            {
                ChangeState(_freezeState);
                _playerMove = null;
                return;
            }
            if (_currentState == _freezeState) return;
            if (_currentState != _chaseState)
            {
                ChangeState(_chaseState);
                return;
            }
            _chaseState.GetPlayerPos(_playerMove.transform.position);
        }
    }
    
    public void GetPlayerMove(PlayerMove playerMove)
    {
        if (playerMove) _playerMove = playerMove;
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
}
