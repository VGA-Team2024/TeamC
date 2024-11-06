using UnityEngine;

/// <summary> 敵の追跡ステート </summary>
public class EnemyChaseState : IEnemyState
{
    private EnemyBase _enemyBase;
    private readonly Animator _animator;
    private readonly int _chase = Animator.StringToHash("Chase");
    private readonly Transform _transform;
    private readonly float _speed;
    private Vector2 _playerPos;
    private readonly bool _isFly;
    
    public EnemyChaseState(EnemyBase enemyBase, Animator animator, Transform transform, float speed, bool isFly)
    {
        _enemyBase = enemyBase;
        _animator = animator;
        _transform = transform;
        _speed = speed;
        _isFly = isFly;
    }
    
    public void Enter()
    {
        if (_animator) _animator.SetBool(_chase, true);
    }

    public void Execute()
    {
        _transform.position = 
            Vector2.MoveTowards(_transform.position, _isFly ? _playerPos : new Vector2(_playerPos.x, _transform.position.y), _speed * Time.deltaTime);

        Direction();
        
        if (_isFly)
        {
            var dir = (Vector2)_transform.position - _playerPos;
            _transform.rotation = Quaternion.FromToRotation(Vector2.left, dir);
        }
    }

    public void Exit()
    {
        _animator.SetBool(_chase, false);
    }

    public void GetPlayerPos(Vector2 pos)
    {
        _playerPos = pos;
    }

    private void Direction()
    {
        _transform.eulerAngles = new Vector2(0, _playerPos.x > _transform.position.x ? 0 : 180);
    }
}
