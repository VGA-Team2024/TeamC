using UnityEngine;

/// <summary> ある範囲に入ったらジャンプ攻撃する敵 </summary>
public class JumpNormalBear : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("接触時攻撃力")] private int _collideDamage;
    [SerializeField, Header("巡回する範囲")] private float _patrolArea;
    [SerializeField, Header("Playerを攻撃した後次の攻撃が可能になるまでの時間")] private int _freezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("ジャンプのスピード")] private float _jumpSpeed;
    [SerializeField, Header("ジャンプ攻撃時の限界高度")] private float _jumpHeight;
    
    private EnemyWalkState _walkState;
    private EnemyJumpAttackState _jumpAttackState;
    private EnemyFreezeState _freezeState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        ParticleSystem particle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        Animator animator = gameObject.transform.GetChild(2).GetComponent<Animator>();
        Rigidbody rb = GetComponent<Rigidbody>();
        
        _walkState = new EnemyWalkState(animator, transform, _speed, _patrolArea);
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime);
        _jumpAttackState = new EnemyJumpAttackState(this, _freezeState, animator, transform, _jumpSpeed, _jumpHeight, rb);
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
            if (_currentState == _jumpAttackState || _currentState == _freezeState) return;
            
            transform.eulerAngles = new Vector2(0, _playerMove.transform.position.x > transform.position.x ? 0 : 180);
            _jumpAttackState.GetPlayerPos(_playerMove.transform.position);
            ChangeState(_jumpAttackState);
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
    
    private void OnDrawGizmos()
    {
        if(Application.isPlaying) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + new Vector3(0, 1), Vector3.right * _patrolArea);
    }
}