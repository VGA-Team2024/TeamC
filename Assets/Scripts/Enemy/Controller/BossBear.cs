using System.Linq;
using UnityEngine;

/// <summary> ボスクマ </summary>
public class BossBear : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("接触時攻撃力")] private int _collideDamage;
    [SerializeField, Header("巡回する範囲")] private float _patrolArea;
    [SerializeField, Header("Playerを攻撃した後次の攻撃が可能になるまでの時間")] private int _freezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("ジャンプ攻撃時のスピード")] private float _jumpSpeed;
    [SerializeField, Header("ジャンプ攻撃時の限界高度")] private float _jumpHeight;
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
        ParticleSystem particle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        GameObject attackCollider = gameObject.transform.GetChild(2).gameObject;
        Animator animator = gameObject.transform.GetChild(3).GetComponent<Animator>();
        Rigidbody rb = GetComponent<Rigidbody>();
        _cottons = gameObject.transform.GetChild(4).GetComponent<Cottons>();
        
        _walkState = new EnemyWalkState(animator, transform, _speed, _patrolArea);
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime);
        _attackState = new EnemyAttackState(this, _freezeState, animator, attackCollider);
        _jumpAttackState = new EnemyJumpAttackState(this, _freezeState, animator, transform, _jumpSpeed, _jumpHeight, rb);
        _rushState = new EnemyRushState(this, _freezeState, animator, transform, _rushDistance, _rushSpeed);
        _createState = new EnemyObjectCreateState(this, _freezeState, animator, _cottons);
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
                            var num = EnemyUtility.ProbabilityCalculate(_waitBWeights);
                            if (num == 0) _jumpAttackState.GetPlayerPos(_playerMove.transform.position);
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
                            var num = EnemyUtility.ProbabilityCalculate(_disCWeights);
                            if (num == 1) _jumpAttackState.GetPlayerPos(_playerMove.transform.position);
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
                            var num = EnemyUtility.ProbabilityCalculate(_walkBWeights);
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
                            var num = EnemyUtility.ProbabilityCalculate(_disCWeights);
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

    private int Distance()
    {
        var dis = Mathf.Abs(transform.position.x - _playerMove.transform.position.x);
        return dis <= _disA ? 1 : dis <= _disB ? 2 : 3;
    }

    //プランナーさんの変更時用
    void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        // 距離A,B,C がどのくらいか
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3(transform.position.x + _disA, transform.position.y), 1f);
        Gizmos.DrawSphere(new Vector3(transform.position.x + _disB, transform.position.y), 1f);
        Gizmos.DrawSphere(new Vector3(transform.position.x + _disC, transform.position.y), 1f);

        // 巡回距離
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + new Vector3(0, 1), Vector3.right * _patrolArea);
    }
}
