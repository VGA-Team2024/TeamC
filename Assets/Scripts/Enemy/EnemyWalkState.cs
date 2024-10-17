using UnityEngine;

/// <summary> 敵の巡回ステート </summary>
public class EnemyWalkState : IEnemyState
{
    private EnemyBase _enemyBase = default;
    private readonly Transform _transform = default;
    private readonly float _speed = default;
    private readonly float _patrolArea = default;
    private readonly Vector2 _startPos = default;
    private readonly SpriteRenderer _spriteRenderer = default;

    public EnemyWalkState(EnemyBase enemyBase,Transform transform, float speed, float area, Vector2 pos, SpriteRenderer renderer)
    {
        _enemyBase = enemyBase;
        _transform = transform;
        _speed = speed;
        _patrolArea = area;
        _startPos = pos;
        _spriteRenderer = renderer;
    }
    
    public void Enter()
    {
        
    }

    public void Execute()
    {
        var x = Mathf.PingPong(Time.time * _speed, _patrolArea);
        _transform.position = new Vector2(_startPos.x + x, _startPos.y);
        if (x <= 0.1f && !_spriteRenderer.flipX || x >= _patrolArea - 0.1f && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = _spriteRenderer.flipX != true;
        }
    }

    public void Exit()
    {
        
    }
}
