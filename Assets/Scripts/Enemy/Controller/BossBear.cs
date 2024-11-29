using System.Linq;
using UnityEngine;
using Random = System.Random;

/// <summary> ボスクマ </summary>
public class BossBear : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("接触時攻撃力")] private int _collideDamage;
    [SerializeField, Header("巡回する範囲")] private float _patrolArea;
    [SerializeField, Header("Playerを攻撃した後次の攻撃が可能になるまでの時間")] private int _freezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("ジャンプ攻撃時のスピード")] private float _jumpSpeed;
    [SerializeField, Header("敵の動き方")] private AnimationCurve _animationCurve;
    [SerializeField, Header("突進時の移動距離")] private float _rushDistance;
    [SerializeField, Header("突進時のスピード")] private float _rushSpeed;
    [SerializeField, Header("距離A")] private int _disA;
    [SerializeField, Header("距離B")] private int _disB;
    [SerializeField, Header("距離C")] private int _disC;
    [SerializeField, Header("待機->攻撃時に距離Bにいたときの攻撃のそれぞれの確率"), Range(0, 100)]
    private int[] _waitBWeights = new int[3];
    [SerializeField, Header("攻撃時に距離Cにいたときの攻撃のそれぞれの確率")]
    private int[] _disCWeights = new int[3];
    [SerializeField, Header("歩行->攻撃時に距離Bにいたときの攻撃のそれぞれの確率")]
    private int[] _walkBWeights = new int[3];
    
    private ParticleSystem _particle;
    private Animator _animator;
    private GameObject _attackCollider;
    private Cottons _cottons;

    private EnemyWalkState _walkState;
    private EnemyAttackState _attackState;
    private EnemyJumpAttackState _jumpAttackState;
    private EnemyRushState _rushState;
    private EnemyObjectCreateState _createState;
    private EnemyFreezeState _freezeState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        _particle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        _attackCollider = gameObject.transform.GetChild(2).gameObject;
        _animator = gameObject.transform.GetChild(3).GetComponent<Animator>();
        _cottons = gameObject.transform.GetChild(4).GetComponent<Cottons>();
        
        _walkState = new EnemyWalkState(this, _animator, transform, _speed, _patrolArea);
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime);
        _attackState = new EnemyAttackState(this, _freezeState, _animator, _attackCollider);
        _jumpAttackState = new EnemyJumpAttackState(this, _freezeState, _animator, transform, _jumpSpeed, _animationCurve);
        _rushState = new EnemyRushState(this, _freezeState, _animator, transform, _rushDistance, _rushSpeed);
        _createState = new EnemyObjectCreateState(this, _freezeState,_animator, _cottons);
        _deathState = new EnemyDeathState(this, _particle, gameObject);
    }

    protected override void OnUpdate()
    {
        if (_isDeath) return;
        
        if (_playerMove)
        {
            if (_currentState == _idleState) // 待機ステート
            {
                transform.eulerAngles = new Vector2(0, _playerMove.transform.position.x > transform.position.x ? 0 : 180);
                switch (Distance())
                {
                    case 1 : // 距離A
                        ChangeState(_attackState);
                        break;
                    case 2 : // 距離B
                        if (_cottons.cottons.Any(_ => _ != null))
                        {
                            var num = ProbabilityCalculate(_waitBWeights);
                            ChangeState(num switch
                            {
                                0 => _jumpAttackState,
                                1 => _attackState,
                                _ => _walkState
                            });
                            break;
                        }
                        ChangeState(_createState);
                        break;
                    case 3 : // 距離C
                        if (_cottons.cottons.Any(_ => _ != null))
                        {
                            var num = ProbabilityCalculate(_disCWeights);
                            ChangeState(num switch
                            {
                                0 => _rushState,
                                1 => _jumpAttackState,
                                _ => _walkState
                            });
                            break;
                        }
                        ChangeState(_createState);
                        break;
                }
            }

            if (_currentState == _walkState) // 歩行ステート
            {
                transform.eulerAngles = new Vector2(0, _playerMove.transform.position.x > transform.position.x ? 0 : 180);
                switch (Distance())
                {
                    case 1 : // 距離A
                        ChangeState(_attackState);
                        break;
                    case 2 : // 距離B
                        if (_cottons.cottons.Any(_ => _ != null))
                        {
                            var num = ProbabilityCalculate(_walkBWeights);
                            ChangeState(num switch
                            {
                                0 => _jumpAttackState,
                                1 => _attackState,
                                _ => _freezeState
                            });
                            break;
                        }
                        ChangeState(_createState);
                        break;
                    case 3 : // 距離C
                        if (_cottons.cottons.Any(_ => _ != null))
                        {
                            var num = ProbabilityCalculate(_disCWeights);
                            ChangeState(num switch
                            {
                                0 => _rushState,
                                1 => _jumpAttackState,
                                _ => _freezeState
                            });
                            break;
                        }
                        ChangeState(_createState);
                        break;
                }
            }
        }
        else
        {
            if (_currentState == _idleState)
            {
                ChangeState(_walkState);
            }
        }
        
        if (_hp.CurrentHp <= 0)
        {
            _isDeath = true;
            ChangeState(_deathState);
        }
    }
    
    public void GetPlayerMove(PlayerMove playerMove)
    {
        _playerMove = playerMove;
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
    
    // プランナーさんの変更時用
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawSphere(new Vector3(transform.position.x + _disA, transform.position.y), 1f);
    //     Gizmos.DrawSphere(new Vector3(transform.position.x + _disB, transform.position.y), 1f);
    //     Gizmos.DrawSphere(new Vector3(transform.position.x + _disC, transform.position.y), 1f);
    // }
}
