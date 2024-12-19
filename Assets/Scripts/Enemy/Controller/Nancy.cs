using UnityEngine;

public class Nancy : EnemyBase,IPlayerTarget, ITeleportable
{
    [SerializeField, Header("接触時攻撃力")] private int _collideDamage;
    [SerializeField, Header("Playerを攻撃した後次の攻撃が可能になるまでの時間")] private int _freezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("ジャンプ攻撃時のスピード")] private float _jumpSpeed;
    [SerializeField, Header("ジャンプ攻撃時の限界高度")] private float _jumpHeight;
    [SerializeField, Header("突進時の移動距離")] private float _rushDistance;
    [SerializeField, Header("突進時のスピード")] private float _rushSpeed;
    [SerializeField, Header("何秒歩行をするか")] private int _walkTime;
    [SerializeField, Header("距離A")] private int _disA;
    [SerializeField, Header("距離B")] private int _disB;
    [SerializeField, Header("距離C")] private int _disC;
    [SerializeField, Header("距離Bにいたときの攻撃のそれぞれの確率"), Range(0, 100)]
    private int[] _disBWeights = new int[3];
    [SerializeField, Header("距離Cにいたときの攻撃のそれぞれの確率"), Range(0, 100)]
    private int[] _disCWeights = new int[3];

    private int _attackCount; // 距離A時の前方攻撃の回数制限用
    
    private EnemyChaseState _chaseState; // 歩行ステート
    private EnemyAttackState _attackState;
    private EnemyJumpAttackState _jumpAttackState;
    private EnemyRushState _rushState;
    private EnemySpecialAttackState _specialAttackState;
    private EnemyFreezeState _freezeState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        ParticleSystem particle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        GameObject attackCollider = gameObject.transform.GetChild(2).gameObject;
        GameObject specialAttackCollider = gameObject.transform.GetChild(3).gameObject;
        Animator animator = gameObject.transform.GetChild(4).GetComponent<Animator>();
        Rigidbody rb = GetComponent<Rigidbody>();
        
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime);
        _chaseState = new EnemyChaseState(this, _freezeState, animator, transform, _speed, false, _walkTime);
        _attackState = new EnemyAttackState(this, _freezeState, animator, attackCollider);
        _jumpAttackState = new EnemyJumpAttackState(this, _freezeState, animator, transform, _jumpSpeed, _jumpHeight, rb);
        _rushState = new EnemyRushState(this, _freezeState, animator, transform, _rushDistance, _rushSpeed);
        _specialAttackState = new EnemySpecialAttackState(this, _freezeState, animator, specialAttackCollider);
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
                    if (num == 2) _jumpAttackState.GetPlayerPos(_playerMove.transform.position);
                    if (num == 3) _chaseState.GetPlayerPos(_playerMove.transform.position);
                    ChangeState(num switch
                    {
                        0 => _specialAttackState,
                        1 => _rushState,
                        2 => _jumpAttackState,
                        _ => _chaseState
                    });
                    break;
                }
                case 3: // 距離がCの場合
                {
                    var num = EnemyUtility.ProbabilityCalculate(_disCWeights);
                    _attackCount = 0;
                    if (num == 2) _jumpAttackState.GetPlayerPos(_playerMove.transform.position);
                    if (num == 3) _chaseState.GetPlayerPos(_playerMove.transform.position);
                    ChangeState(num switch
                    {
                        0 => _specialAttackState,
                        1 => _rushState,
                        2 => _jumpAttackState,
                        _ => _chaseState
                    });
                    break;
                }
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

    public void Teleport(Vector3 position) { transform.position = position; }
}
