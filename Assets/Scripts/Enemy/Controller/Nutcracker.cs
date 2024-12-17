using UnityEngine;

public class Nutcracker : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("接触時攻撃力")] private int _collideDamage;
    [SerializeField, Header("巡回する範囲")] private float _patrolArea;
    [SerializeField, Header("近接攻撃の距離")] private float _prickArea;
    [SerializeField, Header("Playerを攻撃した後次の攻撃が可能になるまでの時間")] private int _freezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("飛ばすオブジェクトのOffSet")] private Vector2 _offSet;
    [SerializeField, Header("遠距離攻撃で飛ばされるオブジェクト")] private GameObject _bullet;
    
    private EnemyWalkState _walkState;
    private EnemyAttackState _attackState;
    private EnemyShootState _shootState;
    private EnemyFreezeState _freezeState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        ParticleSystem particle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        GameObject attackCollider = gameObject.transform.GetChild(2).gameObject;
        Animator animator = gameObject.transform.GetChild(3).GetComponent<Animator>();
        _walkState = new EnemyWalkState(animator, transform, _speed, _patrolArea);
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime);
        _attackState = new EnemyAttackState(this, _freezeState, animator, attackCollider);
        _shootState = new EnemyShootState(this, _freezeState, animator, transform, _offSet, _bullet);
        _deathState = new EnemyDeathState(this, particle, animator, gameObject);
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
            if (_currentState != _idleState && _currentState != _walkState) return;
            if ((transform.position - _playerMove.transform.position).sqrMagnitude < _prickArea * _prickArea)
            {
                ChangeState(_attackState);
            }
            else
            {
                transform.eulerAngles = new Vector2(0, _playerMove.gameObject.transform.position.x > transform.position.x ? 180 : 0);
                _shootState.GetPlayerPos(_playerMove.transform.position);
                ChangeState(_shootState);
            }
            
        }
        else
        {
            if (_currentState == _idleState)
            {
                ChangeState(_walkState);
            }
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
