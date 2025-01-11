using UnityEngine;

public class EnemyPatrolState : IEnemyState
{
    private readonly Animator _animator;
    private readonly int _walk = Animator.StringToHash("Walk");
    private readonly Transform _transform;
    private readonly float _speed;
    private readonly float _patrolArea;
    private readonly Vector2 _startPos;
    private Vector2 _direction;
    
    public EnemyPatrolState(Animator animator, Transform transform, float speed, float area)
    {
        _animator = animator;
        _transform = transform;
        _startPos = transform.position;
        _speed = speed;
        _patrolArea = area;
    }
    
    public void Enter()
    {
        
    }

    public void Execute()
    {
        Direction();
        Patrol();
    }

    public void Exit()
    {
        
    }

    private void Patrol()
    {
        _transform.Translate(_direction * (Time.deltaTime * _speed));
    }

    private void Direction()
    {
        if (_transform.position.y >= _startPos.y - 0.01f)
        {
            _direction = Vector2.down;
        }

        if (_transform.position.y <= _startPos.y - _patrolArea + 0.01f)
        {
            _direction = Vector2.up;
        }
    }
}
