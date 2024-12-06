using UnityEngine;

public class Bat : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("空中か")] private bool _isFly;
    [SerializeField, Header("接触時攻撃力")] private int _collideDamage;
    [SerializeField, Header("止まる時間")] private int _freezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("索敵をやめる距離")] private float _canselDis;
    [SerializeField, Header("次の遠距離攻撃までの時間")] private float _shootTime;
    [SerializeField, Header("遠距離攻撃で飛ばされるオブジェクト")] private GameObject _bullet;
    
    private ParticleSystem _particle;
    private Animator _animator;
    private float _shootTimer;
    
    private EnemyChaseState _chaseState;
    private EnemyShootState _shootState;
    private EnemyFreezeState _freezeState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        _particle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();
        
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime);
        _chaseState = new EnemyChaseState(this, _animator, transform, _speed, _isFly);
        _shootState = new EnemyShootState(this, _freezeState, _animator, transform, _bullet);
        _deathState = new EnemyDeathState(this, _particle, gameObject);
    }

    protected override void OnUpdate()
    {
        if (_isDeath) return;
        
        if (_playerMove)
        {
            if (_currentState == _freezeState || _currentState == _shootState) return;
            if ((transform.position - _playerMove.transform.position).sqrMagnitude > _canselDis * _canselDis)
            {
                ChangeState(_freezeState);
                _playerMove = null;
                return;
            }
            
            if (_currentState != _chaseState)
            {
                ChangeState(_chaseState);
                return;
            }

            _shootTimer += Time.deltaTime;

            if (_shootTimer >= _shootTime)
            {
                _shootTimer = 0;
                _shootState.GetPlayerPos(_playerMove.transform.position);
                ChangeState(_shootState);
            }
            _chaseState.GetPlayerPos(_playerMove.transform.position);
        }
        
        if (_hp.CurrentHp <= 0)
        {
            _isDeath = true;
            ChangeState(_deathState);
        }
    }
    
    public void GetPlayerMove(PlayerMove playerMove)
    {
        if (playerMove) _playerMove = playerMove;
    }
    
    private void OnCollisionStay(Collision other)
    {
        if (_currentState == _freezeState) return;
        if(other.gameObject.CompareTag(_playerTag) && other.gameObject.TryGetComponent(out IDamageable dmg))
        {
            dmg.TakeDamage(_collideDamage);
            ChangeState(_freezeState);
        }
    }
}
