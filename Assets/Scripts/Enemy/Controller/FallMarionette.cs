using UnityEngine;

public class FallMarionette : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("接触時攻撃力")] private int _collideDamage;
    [SerializeField, Header("ぶつかった後当たり判定がなくなる時間")] private int _freezeTime;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    [SerializeField, Header("停止位置")] private GameObject _stopPoint;
    [SerializeField, Header("出現時のスピード")] private float _fallSpeed;
    [SerializeField, Header("親オブジェクト")] private GameObject _obj;

    private bool _isActed;
    
    private EnemyFreezeState _freezeState;
    private EnemyFallState _fallState;
    private EnemyDeathState _deathState;
    
    protected override void OnStart()
    {
        ParticleSystem particle = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        Animator animator = gameObject.transform.GetChild(1).GetComponent<Animator>();
        
        _freezeState = new EnemyFreezeState(this, _idleState, _freezeTime);
        _fallState = new EnemyFallState(this, _freezeState, transform, _stopPoint, _fallSpeed);
        _deathState = new EnemyDeathState(this, particle, animator, _obj);
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
            ChangeState(_fallState);
            _playerMove = null;
        }
    }

    public void GetPlayerMove(PlayerMove playerMove)
    {
        if (_isActed) return;
        _playerMove = playerMove;
        _isActed = true;
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
