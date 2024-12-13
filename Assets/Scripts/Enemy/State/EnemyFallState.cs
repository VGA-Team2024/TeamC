using UnityEngine;

public class EnemyFallState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly Transform _transform;
    private readonly GameObject _point;
    private readonly float _speed;
    
    public EnemyFallState(EnemyBase enemyBase, EnemyFreezeState freezeState,Transform transform, GameObject point, float speed)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _transform = transform;
        _point = point;
        _speed = speed;
    }
    
    public void Enter()
    {
        
    }

    public void Execute()
    {
        Fall();
        if (_transform.position.y <= _point.transform.position.y)
        {
            _enemyBase.ChangeState(_freezeState);
        }
    }

    public void Exit()
    {
        
    }

    private void Fall()
    {
        _transform.Translate(-Vector3.up * (Time.deltaTime * _speed));
    }
}
