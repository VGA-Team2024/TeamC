using UnityEngine;

/// <summary> 敵のジャンプ攻撃ステート </summary>
public class EnemyJumpAttackState : IEnemyState, IPlayerTarget
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly Animator _animator;
    private readonly int _jump = Animator.StringToHash("Jump");
    private readonly Transform _transform;
    private readonly Rigidbody _rb;
    private readonly float _speed;
    private readonly float _height;
    private Vector2 _startPos;
    private Vector2 _playerPos;
    private Vector3 _velocity;

    public EnemyJumpAttackState(EnemyBase enemyBase, EnemyFreezeState freezeState, Animator animator, Transform transform, float speed, float height, Rigidbody rb)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _animator = animator;
        _transform = transform;
        _speed = speed;
        _height = height;
        _rb = rb;
    }
    
    public void Enter()
    {
        if (_animator) _animator.SetBool(_jump, true);
        _rb.useGravity = false;
        _startPos = _transform.position;
        _velocity = EnemyUtility.CalculateVelocity(_transform.position, _playerPos, _height);
    }

    public void Execute()
    {
        Move();
        
        if (_velocity.y < 0 && _transform.position.y < _startPos.y + 0.01f)
        {
            _enemyBase.ChangeState(_freezeState);
        }
    }

    public void Exit()
    {
        _rb.useGravity = true;
        if (_animator) _animator.SetBool(_jump, false);
    }

    private void Move()
    {
        _transform.position += _velocity * (Time.deltaTime * _speed);
        _velocity += Physics.gravity * (Time.deltaTime * _speed);
    }

    public void GetPlayerPos(Vector2 playerPos) { _playerPos = playerPos; }
}
