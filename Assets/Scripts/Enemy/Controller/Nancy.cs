using UnityEngine;

public class Nancy : EnemyBase,IPlayerTarget, ITeleportable
{
    [SerializeField, Header("接触時攻撃力")] private int _collideDamage;
    [SerializeField, Header("playerにぶつかった後動けるまでの時間")] private int _freezeTime;
    [SerializeField, Header("特殊攻撃後の待機時間")] private int _specialAttackFreezeTime;
    [SerializeField, Header("前方攻撃後の待機時間")] private int _attackFreezeTime;
    [SerializeField, Header("ジャンプ後の待機時間")] private int _jumpAttackFreezeTime;
    [SerializeField, Header("突進後の待機時間")] private int _rushFreezeTime;
    [SerializeField, Header("Attack2後の待機時間")] private int _waveFreezeTime;
    [SerializeField, Header("Attack3後の待機時間")] private int _fallFreezeTime;
    [SerializeField, Header("Attack4後の待機時間")] private int _crossFreezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("ジャンプ攻撃時のスピード")] private float _jumpSpeed;
    [SerializeField, Header("ジャンプ攻撃時の限界高度")] private float _jumpHeight;
    [SerializeField, Header("突進時の移動距離")] private float _rushDistance;
    [SerializeField, Header("突進時のスピード")] private float _rushSpeed;
    [SerializeField, Header("何秒歩行をするか")] private int _walkTime;
    [SerializeField, Header("距離A")] private int _disA;
    [SerializeField, Header("距離B")] private int _disB;
    [SerializeField, Header("距離C")] private int _disC;
    [SerializeField, Header("特殊攻撃の確率")] private Weight[] _changePosAttackWeights = new Weight[2]
    {
        new Weight("特殊攻撃"),
        new Weight("ミス")
    };
    [SerializeField, Header("距離Bにいたときの攻撃のそれぞれの確率")]
    private Weight[] _disBWeights = new Weight[6]
    {
        new Weight("突進"),
        new Weight("ジャンプ攻撃"),
        new Weight("Attack2"),
        new Weight("Attack3"),
        new Weight("Attack4"),
        new Weight("歩行")
    };
    [SerializeField, Header("距離Cにいたときの攻撃のそれぞれの確率")]
    private Weight[] _disCWeights = new Weight[6]
    {
        new Weight("突進"),
        new Weight("ジャンプ攻撃"),
        new Weight("Attack2"),
        new Weight("Attack3"),
        new Weight("Attack4"),
        new Weight("歩行")
    };

    private int _attackCount; // 距離A時の前方攻撃の回数制限用
    private bool _isChangePosAttacked; // 特殊攻撃済みか
    private bool _canMove; // 飛んでる間は次の攻撃ができないようにする用
    
    private EnemyChaseState _chaseState; // 歩行ステート
    private EnemyAttackState _attackState;
    private EnemyJumpAttackState _jumpAttackState;
    private EnemyRushState _rushState;
    private EnemySpecialAttackState _changePositionState;
    private EnemySpecialAttackState _waveNeedleState; // Attack2
    private EnemyFallNeedleState _fallNeedleState; // Attack3
    private EnemySpecialAttackState _crossNeedleState; // Attack4
    private EnemyFreezeState _freezeState; // 歩行とぶつかった後の待機ステート
    private EnemyFreezeState _specialAttackFreezeState; // 特殊攻撃後の待機ステート
    private EnemyFreezeState _attackFreezeState; // 前方攻撃後の待機ステート
    private EnemyFreezeState _rushFreezeState; // 突進攻撃後の待機ステート
    private EnemyFreezeState _jumpFreezeState; // ジャンプ攻撃後の待機ステート
    private EnemyFreezeState _waveNeedleFreezeState; // Attack2後の待機ステート
    private EnemyFreezeState _fallNeedleFreezeState; // Attack3後の待機ステート
    private EnemyFreezeState _crossNeedleFreezeState; // Attack4後の待機ステート
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        ParticleSystem particle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        GameObject attackCollider = gameObject.transform.GetChild(2).gameObject;
        GameObject specialAttackCollider = gameObject.transform.GetChild(3).gameObject;
        Animator animator = gameObject.transform.GetChild(4).GetComponent<Animator>();
        GameObject crossNeedleCollider = gameObject.transform.GetChild(5).gameObject;
        GameObject fallNeedle = gameObject.transform.GetChild(6).gameObject;
        GameObject waveNeedle = gameObject.transform.GetChild(7).gameObject;
        Rigidbody rb = GetComponent<Rigidbody>();
        
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime); // 歩行とぶつかった後の待機ステート
        _specialAttackFreezeState = new EnemyFreezeState(this, _idleState, _specialAttackFreezeTime); // 特殊攻撃後の待機ステート
        _attackFreezeState = new EnemyFreezeState(this, _idleState, _attackFreezeTime); // 前方攻撃後の待機ステート
        _jumpFreezeState = new EnemyFreezeState(this, _idleState, _jumpAttackFreezeTime); // ジャンプ攻撃後の待機ステート
        _rushFreezeState = new EnemyFreezeState(this, _idleState, _rushFreezeTime); // 突進攻撃後の待機ステート
        _waveNeedleFreezeState = new EnemyFreezeState(this, _idleState, _waveFreezeTime); // Attack2後の待機ステート
        _fallNeedleFreezeState = new EnemyFreezeState(this, _idleState, _fallFreezeTime); // Attack3後の待機ステート
        _crossNeedleFreezeState = new EnemyFreezeState(this, _idleState, _crossFreezeTime); // Attack4後の待機ステート
        
        _chaseState = new EnemyChaseState(this, _freezeState, animator, transform, _speed, false, _walkTime);
        _attackState = new EnemyAttackState(this, _attackFreezeState, animator, attackCollider);
        _jumpAttackState = new EnemyJumpAttackState(this, _jumpFreezeState, animator, transform, _jumpSpeed, _jumpHeight, rb);
        _rushState = new EnemyRushState(this, _rushFreezeState, animator, transform, _rushDistance, _rushSpeed);
        _changePositionState = new EnemySpecialAttackState(this, _specialAttackFreezeState, animator, specialAttackCollider, 0, "SpecialAttack");
        _waveNeedleState = new EnemySpecialAttackState(this, _waveNeedleFreezeState, animator, waveNeedle, 1, "Attack2");
        _fallNeedleState = new EnemyFallNeedleState(this, _fallNeedleFreezeState, animator, fallNeedle, gameObject.transform, rb);
        _crossNeedleState = new EnemySpecialAttackState(this, _crossNeedleFreezeState, animator, crossNeedleCollider, 0, "Attack4");
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

        if (_playerMove && _canMove)
        {
            if (_currentState != _idleState) return;
            
            transform.eulerAngles = new Vector2(0, _playerMove.transform.position.x > transform.position.x ? 0 : 180);

            if (!_isChangePosAttacked && EnemyUtility.ProbabilityCalculate(_changePosAttackWeights) == 0)
            {
                ChangeState(_changePositionState);
                _isChangePosAttacked = true;
                return;
            } // 特殊攻撃は毎回抽選

            _isChangePosAttacked = false;
            
            switch (Distance())
            {
                case 1 : // 距離がAの場合
                    if (_attackCount >= 4) // 前方攻撃3回連続でやっていたら突進
                    {
                        ChangeState(_rushState);
                        _attackCount = 0;
                        return;
                    }
                
                    ChangeState(_attackState);
                    _attackCount++;
                    break;
                case 2: // 距離がBの場合
                {
                    var num = EnemyUtility.ProbabilityCalculate(_disBWeights);
                    _attackCount = 0;
                    if (num == 1) _jumpAttackState.GetPlayerPos(_playerMove.transform.position);
                    if (num == 2) _chaseState.GetPlayerPos(_playerMove.transform.position);
                    ChangeState(num switch
                    {
                        0 => _rushState,
                        1 => _jumpAttackState,
                        2 => _waveNeedleState,
                        3 => _fallNeedleState,
                        4 => _crossNeedleState,
                        _ => _chaseState
                    });
                }
                    break;
                case 3: // 距離がCの場合
                {
                    var num = EnemyUtility.ProbabilityCalculate(_disCWeights);
                    _attackCount = 0;
                    if (num == 1) _jumpAttackState.GetPlayerPos(_playerMove.transform.position);
                    if (num == 2) _chaseState.GetPlayerPos(_playerMove.transform.position);
                    ChangeState(num switch
                    {
                        0 => _rushState,
                        1 => _jumpAttackState,
                        2 => _waveNeedleState,
                        3 => _fallNeedleState,
                        4 => _crossNeedleState,
                        _ => _chaseState
                    });
                }
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
    
    private void OnCollisionEnter(Collision other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Ground") _canMove = true;
    }

    private void OnCollisionExit(Collision other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Ground") _canMove = false;
    }
    
    private int Distance()
    {
        var dis = Mathf.Abs(transform.position.x - _playerMove.transform.position.x);
        return dis <= _disA ? 1 : dis <= _disB ? 2 : 3;
    }

    public void Teleport(Vector3 position) { transform.position = position; }
    
    void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        // 距離A,B,C がどのくらいか
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3(transform.position.x + _disA, transform.position.y), 1f);
        Gizmos.DrawSphere(new Vector3(transform.position.x + _disB, transform.position.y), 1f);
        Gizmos.DrawSphere(new Vector3(transform.position.x + _disC, transform.position.y), 1f);
    }
}
