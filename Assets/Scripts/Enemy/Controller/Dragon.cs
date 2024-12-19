using UnityEngine;

public class Dragon : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("接触時攻撃力")] private int _collideDamage;
    [SerializeField, Header("Playerを攻撃した後次の攻撃が可能になるまでの時間")] private int _freezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("ジャンプ攻撃時のスピード")] private float _jumpSpeed;
    [SerializeField, Header("ジャンプ攻撃時の限界高度")] private float _jumpHeight;
    [SerializeField, Header("突進時の移動距離")] private float _rushDistance;
    [SerializeField, Header("突進時のスピード")] private float _rushSpeed;
    [SerializeField, Header("何秒歩行をするか")] private int _walkTime;
    [SerializeField, Header("どのくらい飛ぶか")] private float _height;
    [SerializeField, Header("ブレスの弾")] private GameObject _breathBullet;
    [SerializeField, Header("飛びブレスのoffset")] private Vector2 _offSet;
    [SerializeField, Header("距離A")] private int _disA;
    [SerializeField, Header("距離B")] private int _disB;
    [SerializeField, Header("距離C")] private int _disC;
    [SerializeField, Header("距離Bにいたときの攻撃のそれぞれの確率"), Range(0, 100)]
    private int[] _disBWeights = new int[3];
    [SerializeField, Header("距離Cにいたときの攻撃のそれぞれの確率"), Range(0, 100)]
    private int[] _disCWeights = new int[3];

    private bool _canMove; // 飛んでる間は次の攻撃ができないようにする用
    
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
        _jumpAttackState = new EnemyJumpAttackState(this, _freezeState, animator, transform, _jumpSpeed, _jumpHeight, rb);
        _rushState = new EnemyRushState(this, _freezeState, animator, transform, _rushDistance, _rushSpeed);
        _shootState = new EnemyShootState(this, _freezeState, animator, transform, _offSet, _breathBullet); // 地上ブレス
        _breathState = new EnemyBreathState(this, _freezeState, animator, breaths); // 飛びブレス
        _flyState = new EnemyFlyState(this, _breathState, animator, transform, rb, _height);
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
            if (_currentState != _idleState && _currentState != _chaseState) return;
            transform.eulerAngles = new Vector2(0, _playerMove.transform.position.x > transform.position.x ? 0 : 180);
            switch (Distance())
            {
                case 1: // 距離A
                    ChangeState(_attackState);
                    break;
                case 2: // 距離B
                {
                    var num = EnemyUtility.ProbabilityCalculate(_disBWeights);
                    if (num == 0) _shootState.GetPlayerPos(_playerMove.transform.position);
                    ChangeState(num switch
                    {
                        0 => _shootState,
                        1 => _rushState,
                        2 => _jumpAttackState,
                        _ => _currentState == _idleState ? _chaseState : _idleState
                    });
                }
                    break;
                case 3: // 距離C
                {
                    var num = EnemyUtility.ProbabilityCalculate(_disCWeights);
                    if (num == 0) _shootState.GetPlayerPos(_playerMove.transform.position);
                    ChangeState(num switch
                    {
                        0 => _shootState,
                        1 => _rushState,
                        _ => _flyState
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
        if (LayerMask.LayerToName(other.gameObject.layer) == "Ground") _canMove = true;
        if (!other.gameObject.CompareTag(_playerTag)) return;
        if(other.gameObject.TryGetComponent(out IDamageable dmg) && other.gameObject.TryGetComponent(out IBlowable blo))
        {
            dmg.TakeDamage(_collideDamage);
            blo.BlownAway(transform.position);
            ChangeState(_freezeState);
        }
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
}
