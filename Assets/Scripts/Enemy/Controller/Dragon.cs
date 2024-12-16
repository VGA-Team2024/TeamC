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
    [SerializeField, Header("飛びブレスのoffset")] private Vector2 _offSet;
    [SerializeField, Header("距離A")] private int _disA;
    [SerializeField, Header("距離B")] private int _disB;
    [SerializeField, Header("距離C")] private int _disC;
    [SerializeField, Header("距離Bにいたときの攻撃のそれぞれの確率"), Range(0, 100)]
    private int[] _disBWeights = new int[3];
    [SerializeField, Header("距離Cにいたときの攻撃のそれぞれの確率"), Range(0, 100)]
    private int[] _disCWeights = new int[3];

    private EnemyChaseState _chaseState; // 歩行ステート
    private EnemyAttackState _attackState;
    private EnemyJumpAttackState _jumpAttackState;
    private EnemyRushState _rushState;
    private EnemyShootState _shootState;
    private EnemyBreathState _breathState;
    private EnemyFlyState _flyState;
    private EnemyFreezeState _freezeState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        ParticleSystem particle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        GameObject attackCollider = gameObject.transform.GetChild(2).gameObject;
        GameObject breaths = gameObject.transform.GetChild(3).gameObject;
        Animator animator = gameObject.transform.GetChild(4).GetComponent<Animator>();
        Rigidbody rb = GetComponent<Rigidbody>();

        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime);
        _chaseState = new EnemyChaseState(this, _freezeState, animator, transform, _speed, false, _walkTime);
        _attackState = new EnemyAttackState(this, _freezeState, animator, attackCollider);
        _jumpAttackState = new EnemyJumpAttackState(this, _freezeState, animator, transform, _jumpSpeed, _animationCurve);
        _rushState = new EnemyRushState(this, _freezeState, animator, transform, _rushDistance, _rushSpeed);
        _shootState = new EnemyShootState(this, _freezeState, animator, transform, _offSet, _breathBullet); // 飛びブレス
        _breathState = new EnemyBreathState(this, _freezeState, animator, breaths); // 陸上ブレス
        _flyState = new EnemyFlyState(this, _shootState, animator, transform, rb, _height);
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
            if (_currentState != _idleState && _currentState != _chaseState) return;
            transform.eulerAngles = new Vector2(0, _playerMove.transform.position.x > transform.position.x ? 0 : 180);
            switch (Distance())
            {
                case 1: // 距離A
                    ChangeState(_attackState);
                    break;
                case 2: // 距離B
                    ChangeState(ProbabilityCalculate(_disBWeights) switch
                    {
                        0 => _breathState,
                        1 => _rushState,
                        2 => _jumpAttackState,
                        _ => _currentState == _idleState ? _chaseState : _idleState
                    });
                    break;
                case 3: // 距離C
                    var num = ProbabilityCalculate(_disCWeights);
                    if (num == 2) _flyState.GetPlayerPos(_playerMove.transform.position);
                    ChangeState(num switch
                    {
                        0 => _breathState,
                        1 => _rushState,
                        _ => _flyState
                    });
                    break;
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
    
    private int Distance()
    {
        var dis = Mathf.Abs(transform.position.x - _playerMove.transform.position.x);

        return dis <= _disA ? 1 : dis <= _disB ? 2 : 3;
    }

    private int ProbabilityCalculate(int[] weights)
    {
        var rnd = new Random().Next(1, 101);
        var cumulative = 0;
        var index = 0;
        for (var i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (rnd > cumulative) continue;
            index = i;
            break;
        }

        return index;
    }
}
