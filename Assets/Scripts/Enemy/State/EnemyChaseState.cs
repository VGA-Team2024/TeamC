using UnityEngine;

/// <summary> 敵の追跡ステート </summary>
public class EnemyChaseState : IEnemyState
{
    private EnemyBase _enemyBase;
    private readonly Transform _transform;
    private readonly float _speed;
    private readonly SpriteRenderer _spriteRenderer;
    private Vector2 _playerPos;
    private readonly bool _isFly;
    
    public EnemyChaseState(EnemyBase enemyBase, Transform transform, float speed, SpriteRenderer renderer, bool isFly)
    {
        _enemyBase = enemyBase;
        _transform = transform;
        _speed = speed;
        _spriteRenderer = renderer;
        _isFly = isFly;
    }
    
    public void Enter()
    {
        
    }

    public void Execute()
    {
        _transform.position = 
            Vector2.MoveTowards(_transform.position, _isFly ? _playerPos : new Vector2(_playerPos.x, _transform.position.y), _speed * Time.deltaTime);

        if (_isFly)
        {
            _spriteRenderer.flipY = _playerPos.x > _transform.position.x;
            var dir = (Vector2)_transform.position - _playerPos;
            _transform.rotation = Quaternion.FromToRotation(Vector2.left, dir);
        }
        else
        {
            _spriteRenderer.flipX = _playerPos.x > _transform.position.x;
        }
        
    }

    public void Exit()
    {
        
    }

    public void GetPlayerPos(Vector2 pos)
    {
        _playerPos = pos;
    }
}
