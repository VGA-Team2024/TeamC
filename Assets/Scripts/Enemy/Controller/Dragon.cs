using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class Dragon : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("接触時攻撃力")] private int _collideDamage;
    [SerializeField, Header("Playerを攻撃した後次の攻撃が可能になるまでの時間")] private int _freezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("ジャンプ攻撃時のスピード")] private float _jumpSpeed;
    [SerializeField, Header("敵の動き方")] private AnimationCurve _animationCurve;
    [SerializeField, Header("突進時の移動距離")] private float _rushDistance;
    [SerializeField, Header("突進時のスピード")] private float _rushSpeed;
    [SerializeField, Header("何秒歩行をするか")] private int _walkTime;
    [SerializeField, Header("どのくらい飛ぶか")] private float _height;
    [FormerlySerializedAs("_obj")] [SerializeField, Header("ブレスの弾")] private GameObject _breathBullet;

    private bool _isFly;
    
    private EnemyChaseState _chaseState; // 歩行ステート
    private EnemyAttackState _attackState;
    private EnemyJumpAttackState _jumpAttackState;
    private EnemyRushState _rushState;
    private EnemyFlyState _flyState;
    private EnemyBreathState _breathState;
    private EnemyFreezeState _freezeState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        ParticleSystem particle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        GameObject attackCollider = gameObject.transform.GetChild(2).gameObject;
        GameObject breaths = gameObject.transform.GetChild(3).gameObject;
        Animator animator = GetComponent<Animator>();

        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime);
        _chaseState = new EnemyChaseState(this, _freezeState, animator, transform, _speed, false, _walkTime);
        _attackState = new EnemyAttackState(this, _freezeState, animator, attackCollider);
        _jumpAttackState = new EnemyJumpAttackState(this, _freezeState, animator, transform, _jumpSpeed, _animationCurve);
        _rushState = new EnemyRushState(this, _freezeState, animator, transform, _rushDistance, _rushSpeed);
        _flyState = new EnemyFlyState(this, _freezeState, animator, transform, _height);
        _breathState = new EnemyBreathState(this, _freezeState, animator, transform, _breathBullet, breaths);
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
            if (_currentState != _idleState) return;
            
            transform.eulerAngles = new Vector2(0, _playerMove.transform.position.x > transform.position.x ? 0 : 180);
            
            var rnd = new Random().Next(0, 5);
            switch (rnd)
            {
                case 0:
                    _breathState.GetPlayerPos(_playerMove.transform.position);
                    _breathState.IsFly(_isFly);
                    ChangeState(_breathState);
                    break;
                case 1:
                    ChangeState(_jumpAttackState);
                    break;
                case 2:
                    ChangeState(_rushState);
                    break;
                case 3:
                    ChangeState(_attackState);
                    break;
                case 4:
                    _chaseState.GetPlayerPos(_playerMove.transform.position);
                    ChangeState(_chaseState);
                    break;
                // case 5:
                //     if (_isFly) return;
                //     ChangeState(_flyState);
                //     _isFly = true;
                //     break;
            }
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
            dmg.TakeDamage(_collideDamage);
            blo.BlownAway(transform.position);
            ChangeState(_freezeState);
        }
    }
}
