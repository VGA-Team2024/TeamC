using UnityEngine;

/// <summary> 敵の巡回ステート </summary>
public class EnemyWalkState : IEnemyState
{
    private EnemyBase _enemyBase;
    private readonly Animator _animator;
    private readonly int _walk = Animator.StringToHash("Walk");
    private readonly Transform _transform;
    private readonly float _speed;
    private readonly float _patrolArea;
    private readonly Vector2 _startPos;
    private float _patrolTimer;
    private bool _onLastPos;

    public EnemyWalkState(EnemyBase enemyBase,Animator animator, Transform transform, float speed, float area)
    {
        _enemyBase = enemyBase;
        _animator = animator;
        _transform = transform;
        _startPos = transform.position;
        _speed = speed;
        _patrolArea = area;
    }
    
    public void Enter()
    {
        _animator.SetBool(_walk, true);
    }

    public void Execute()
    {
        Direction();
        Walk();
    }

    public void Exit()
    {
        _animator.SetBool(_walk, false);
    }
    
    private void Walk()
    {
        _transform.Translate(Vector3.right * (Time.deltaTime * _speed));
    }

    private void Direction()
    {
        if (_transform.position.x <= _startPos.x + 0.01f)
        {
            _transform.eulerAngles = new Vector2(0, 0);
        }

        if (_transform.position.x >= _startPos.x + _patrolArea - 0.1f)
        {
            _transform.eulerAngles = new Vector2(0, 180);
        }
    }
}
